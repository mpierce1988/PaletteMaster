using PaletteMaster.Models;
using PaletteMaster.Models.DTO.FileManagement;
using PaletteMaster.Services.Utilities;

namespace PaletteMaster.Services.FileManagement;

public class FileManagementService : IFileManagementService
{
    #region Public Methods
    
    /// <summary>
    /// Load all files in a folder and its subfolders 
    /// </summary>
    /// <param name="request">The request object containing the path to the folder to load files from</param>
    /// <returns>
    /// A response object containing the path to the folder and a list of FileResponses for each file in the folder
    /// </returns>
    public async Task<Result<LoadFolderResponse, HandledException>> LoadFolderAsync(LoadFolderRequest request)
    {
        try
        {
            if (!ValidatorUtility.TryValidateObject(request, out var validationResults))
            {
                return new HandledException(validationResults);
            }
            
            // Load all files in the directory, including subfolders
            
            string[] files = Directory.GetFiles(request.Path, "*.*", SearchOption.AllDirectories);
            
            // For each file path, create a fileResponse object and add it to the response
            LoadFolderResponse response = new(request.Path);

            await Task.Run(() =>
            {
                foreach (string file in files)
                {
                    string fileName = Path.GetFileName(file);
                    if (!IsSupportedFileType(fileName)) continue;
                
                    FileStream fileStream = File.OpenRead(file);
                    FileResponse fileResponse = new(fileName, file, fileStream);
                    response.Files.Add(fileResponse);
                }
            });
            
            return response;
        }
        catch (Exception e)
        {
            return new HandledException(e.Message);
        }
    }

    /// <summary>
    /// Save files to a folder 
    /// </summary>
    /// <param name="request">The request object containing the output path and files to save</param>
    /// <returns>A response object containing the output path and the number of files saved</returns>
    public async Task<Result<SaveFilesToFolderResponse, HandledException>> SaveFilesToFolderAsync(SaveFilesToFolderRequest request)
    {
        try
        {
            if (!ValidatorUtility.TryValidateObject(request, out var validationResults))
            {
                return new HandledException(validationResults);
            }
            
            // Save files to directory
            foreach (FileToSave file in request.FilesToSave)
            {
                EnsureDirectoryExists(request.OutputPath, file);

                string finalPath = GetFinalOutputPath(request.OutputPath, file);

                await using FileStream fileStream = File.Create(finalPath);
                await file.Stream.CopyToAsync(fileStream);
            }
            
            return new SaveFilesToFolderResponse(request.OutputPath, request.FilesToSave.Count);
        }
        catch (Exception e)
        {
            return new HandledException(e.Message);
        }
    }
    
    #endregion
    
    #region Private Methods
    
    /// <summary>
    /// Check if the file is a supported file type 
    /// </summary>
    /// <param name="fileName">The name of the file to check</param>
    /// <returns>True if the file is a supported file type, false otherwise</returns>
    private bool IsSupportedFileType(string fileName)
    {
        return fileName.Contains(".png");
    }

    /// <summary>
    /// Get the final output path for the file
    /// </summary>
    /// <param name="initialOutputPath">The initial output path root directory</param>
    /// <param name="file">FileToSave which includes the filename and relative path</param>
    /// <returns>The final output path for the file in the output directory, including subdirectories</returns>
    private string GetFinalOutputPath(string initialOutputPath, FileToSave file)
    {
        string finalPath = initialOutputPath;
        
        // Use index from end expression to ensure last character is a slash
        if(finalPath[^1] != '/')
        {
            finalPath += "/";
        }

        if (file.RelativePath.TrimStart('/') == file.FileName)
        {
            finalPath += file.FileName;
        }
        else
        {
            //string relativePathWithoutFilename = file.RelativePath.TrimStart('/').Replace(file.FileName, "");
            finalPath += file.RelativePath.TrimStart('/');
        }

        return finalPath;
    }

    /// <summary>
    /// Ensure the directory exists for the file 
    /// </summary>
    /// <param name="initialOutputPath">The initial output path root directory</param>
    /// <param name="file">FileToSave which includes the filename and relative path</param>
    private void EnsureDirectoryExists(string initialOutputPath, FileToSave file)
    {
        // Ensure initialOutputPath ends in a slash
        if (initialOutputPath[^1] != '/')
        {
            initialOutputPath += "/";
        }
        
        string relativePathWithoutFilename = file.RelativePath.Replace(file.FileName, "");
        // Get relative path without the filename
        string fullPath = string.Concat(initialOutputPath, relativePathWithoutFilename);
               
        // Create directory if it doesn't exist
        if (!Directory.Exists(fullPath))
        {
            Directory.CreateDirectory(fullPath);
        }
    }
    
    #endregion
}