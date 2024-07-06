using CommunityToolkit.Maui.Storage;
using PaletteMaster.Models;
using PaletteMaster.Models.DTO;

namespace PaletteMaster.Presentation.Services;

public class StandardOSFilePickerService : IFilePickerService
{
    private readonly IFileSaver _fileSaver;
    
    public StandardOSFilePickerService(IFileSaver fileSaver)
    {
        _fileSaver = fileSaver;
    }
    
    public async Task<Stream?> PickFileAsync()
    {
        var fileResult = await RequestFileResult();
        
        if (fileResult == null)
        {
            return null;
        }

        return await fileResult.OpenReadAsync();
    }
    
    public async Task<(Stream?, string)> PickFileAndNameAsync()
    {
        FileResult? fileResult = await RequestFileResult();
        
        if (fileResult == null)
        {
            return (null, string.Empty);
        }

        return (await fileResult.OpenReadAsync(), fileResult.FileName);
    }

    private async Task<FileResult?> RequestFileResult()
    {
        var fileResult = await FilePicker.PickAsync(new PickOptions()
        {
            PickerTitle = "Please select your Palette file",
            FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>()
            {
                { DevicePlatform.WinUI, new []{ "txt", "hex", "TXT", "HEX", "png", "PNG"}}
            })
            
        });
        return fileResult;
    }
    
    public async Task<Result<SaveFileResponse, HandledException>> SaveFileAsync(SaveFileRequest request)
    {
        try
        {
            if (request.FileStream is null)
            {
                return new HandledException("File Stream is null");
            }
            
            if (string.IsNullOrWhiteSpace(request.FileName))
            {
                return new HandledException("File Name is null or empty");
            }
            
            request.FileStream.Position = 0;

            var fileSaverResult = await _fileSaver.SaveAsync(request.FileName, request.FileStream);

            if (!fileSaverResult.IsSuccessful)
            {
                return new HandledException(
                    $"Failed to save file with error: {fileSaverResult.Exception?.Message ?? "Unknown"}");
            }

            return new SaveFileResponse(request.FileName, fileSaverResult.FilePath);
        }
        catch (Exception e)
        {
            return new HandledException(e.Message);
        }
    }
}