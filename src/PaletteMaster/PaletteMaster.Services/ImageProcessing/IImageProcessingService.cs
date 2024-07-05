using PaletteMaster.Models;
using PaletteMaster.Models.DTO.ImageProcessing;

namespace PaletteMaster.Services.ImageProcessing;

public interface IImageProcessingService
{
    public Task<Result<ImageProcessingResponse, HandledException>> ProcessImageAsync(ImageProcessingRequest request);
}