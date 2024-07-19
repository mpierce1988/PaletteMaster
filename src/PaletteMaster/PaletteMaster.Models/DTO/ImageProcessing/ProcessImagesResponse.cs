namespace PaletteMaster.Models.DTO.ImageProcessing;

public class ProcessImagesResponse
{
    public List<ProcessImageResponse> ProcessedImages { get; set; } = new();
    
    public ProcessImagesResponse(List<ProcessImageResponse> processedImages)
    {
        ProcessedImages = processedImages;
    }
}