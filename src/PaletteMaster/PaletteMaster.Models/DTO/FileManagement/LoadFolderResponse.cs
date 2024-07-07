using PaletteMaster.Models.Domain;
using PaletteMaster.Models.DTO.ImageProcessing;

namespace PaletteMaster.Models.DTO.FileManagement;

public class LoadFolderResponse
{
    public string Path { get; set; }

    public List<FileResponse> Files { get; set; } = new();
    
    public LoadFolderResponse(string path)
    {
        Path = path;
    }

    public ImageFolderProcessingRequest ToImageFolderProcessingRequest(List<Color>? colors = null)
    {
        var imageFolderProcessingRequest = new ImageFolderProcessingRequest();
        colors ??= new();
        imageFolderProcessingRequest.Colors = colors;
        
        foreach (FileResponse file in Files)
        {
            var imageProcessingRequest = new ImageProcessingRequest()
            {
                FileName = file.FileName,
                FilePath = file.Path,
                RelativePath = file.Path.Replace(Path, ""),
                FileStream = file.FileStream,
                Colors = colors
            };
            
            imageFolderProcessingRequest.ImagesToProcess.Add(imageProcessingRequest);
        }

        return imageFolderProcessingRequest;
    }

}