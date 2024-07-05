using PaletteMaster.Models;
using PaletteMaster.Models.DTO;

namespace PaletteMaster.Presentation.Services;

public interface IFilePickerService
{
    public Task<Stream?> PickFileAsync();
    public Task<(Stream?, string, string)> PickFileAndNameAsync();
    
    public Task<Result<SaveFileResponse, HandledException>> SaveFileAsync(byte[] fileBytes, string fileName);
}