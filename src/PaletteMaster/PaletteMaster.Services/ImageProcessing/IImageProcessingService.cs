using PaletteMaster.Models;
using PaletteMaster.Models.DTO.ImageProcessing;

namespace PaletteMaster.Services.ImageProcessing;

public interface IImageProcessingService
{
    public Task<Result<ProcessImageResponse, HandledException>> ProcessImageAsync(ProcessImageRequest request);

    public Task<Result<ProcessImagesResponse, HandledException>> ProcessImagesAsync(
        ProcessImagesRequest request);

    public Task<Result<ProcessFolderResponse, HandledException>> ProcessFolderAsync(ProcessFolderRequest request);

}