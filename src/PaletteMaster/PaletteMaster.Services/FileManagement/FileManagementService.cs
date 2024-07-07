using PaletteMaster.Models;
using PaletteMaster.Models.DTO.FileManagement;
using PaletteMaster.Services.Utilities;

namespace PaletteMaster.Services.FileManagement;

public class FileManagementService : IFileManagementService
{
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
            
            foreach (string file in files)
            {
                string fileName = Path.GetFileName(file);
                FileStream fileStream = File.OpenRead(file);
                FileResponse fileResponse = new(fileName, file, fileStream);
                response.Files.Add(fileResponse);
            }
            
            return response;
        }
        catch (Exception e)
        {
            return new HandledException(e.Message);
        }
    }

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
                EnsureDirectoryExists(request, file);
                
                using FileStream fileStream = File.Create(Path.Combine(request.OutputPath, file.RelativePath));
                await file.Stream.CopyToAsync(fileStream);
            }
            
            return new SaveFilesToFolderResponse(request.OutputPath, request.FilesToSave.Count);
        }
        catch (Exception e)
        {
            return new HandledException(e.Message);
        }
    }

    private void EnsureDirectoryExists(SaveFilesToFolderRequest request, FileToSave file)
    {
        string relativePathWithoutFilename = file.RelativePath.Replace(file.FileName, "");
        // Get relative path without the filename
        string fullPath = Path.GetFullPath(Path.Combine(request.OutputPath, relativePathWithoutFilename));
               
        // Create directory if it doesn't exist
        if (!Directory.Exists(fullPath))
        {
            Directory.CreateDirectory(fullPath);
        }
    }
}