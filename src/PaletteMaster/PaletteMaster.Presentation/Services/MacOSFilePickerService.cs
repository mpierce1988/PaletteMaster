using System.IO.Compression;
using CommunityToolkit.Maui.Storage;
using LukeMauiFilePicker;
using PaletteMaster.Models;
using PaletteMaster.Models.DTO;

namespace PaletteMaster.Presentation.Services;

public class MacOSFilePickerService : IFilePickerService
{
    private readonly LukeMauiFilePicker.FilePickerService _customFilePicker = new();
    private readonly IFileSaver _fileSaver;
    
    public MacOSFilePickerService(IFileSaver fileSaver)
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
        var fileResult = await RequestFileResult();
        
        if (fileResult == null)
        {
            return (null, string.Empty);
        }

        return (await fileResult.OpenReadAsync(), fileResult.FileName);
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

    private async Task<IPickFile?> RequestFileResult()
    {
        var fileResult = await _customFilePicker.PickFileAsync( 
        
            "Please select your Palette file",
            new Dictionary<DevicePlatform, IEnumerable<string>>()
            {
                { DevicePlatform.MacCatalyst, new []{ "txt", "hex", "TXT", "HEX", "png", "PNG"}}
            }
        );
        return fileResult;
    }
}