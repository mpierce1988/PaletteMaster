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

    public async Task<(Stream?, string, string)> PickFileAndNameAsync()
    {
        var fileResult = await RequestFileResult();
        
        if (fileResult == null)
        {
            return (null, string.Empty, string.Empty);
        }
        
        

        return (await fileResult.OpenReadAsync(), fileResult.FileName, fileResult.FileResult!.FullPath);
    }

    public async Task<Result<SaveFileResponse, HandledException>> SaveFileAsync(byte[] fileBytes, string fileName)
    {
        try
        {
            using MemoryStream contentStream = new(fileBytes);
            contentStream.Position = 0;

            var fileSaverResult = await _fileSaver.SaveAsync(fileName, contentStream);

            if (!fileSaverResult.IsSuccessful)
            {
                return new HandledException(
                    $"Failed to save file with error: {fileSaverResult.Exception?.Message ?? "Unknown"}");
            }

            return new SaveFileResponse(fileName, fileSaverResult.FilePath);
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