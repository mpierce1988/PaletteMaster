using PaletteMaster.Models.DTO.ImageProcessing;

namespace PaletteMaster.Models.DTO.FileManagement;

public class SaveFilesToFolderRequest
{
    public List<FileToSave> FilesToSave { get; set; } = new();
    public string OutputPath { get; set; }
    
    public SaveFilesToFolderRequest(List<FileToSave> filesToSave, string outputPath)
    {
        FilesToSave = filesToSave;
        OutputPath = outputPath;
    }

    public SaveFilesToFolderRequest(ImageFolderProcessingResponse imageFolderProcessingResponse, string outputPath)
    {
        OutputPath = outputPath;
        foreach (var processedImage in imageFolderProcessingResponse.ProcessedImages)
        {
            FileToSave file = new FileToSave(processedImage.FileName, processedImage.RelativePath, processedImage.Stream);
            FilesToSave.Add(file);
        }
    }
}