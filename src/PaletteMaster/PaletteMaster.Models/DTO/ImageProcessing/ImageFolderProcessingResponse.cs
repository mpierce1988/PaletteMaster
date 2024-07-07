namespace PaletteMaster.Models.DTO.ImageProcessing;

public class ImageFolderProcessingResponse
{
    public List<ImageProcessingResponse> ProcessedImages { get; set; } = new();
    
    public ImageFolderProcessingResponse(List<ImageProcessingResponse> processedImages)
    {
        ProcessedImages = processedImages;
    }
}