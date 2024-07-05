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
    
    public async Task<(Stream?, string, string)> PickFileAndNameAsync()
    {
        FileResult? fileResult = await RequestFileResult();
        
        if (fileResult == null)
        {
            return (null, string.Empty, string.Empty);
        }

        return (await fileResult.OpenReadAsync(), fileResult.FileName, fileResult.FullPath);
    }

    private async Task<FileResult?> RequestFileResult()
    {
        var fileResult = await FilePicker.PickAsync(new PickOptions()
        {
            PickerTitle = "Please select your Palette file",
            FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>()
            {
                { DevicePlatform.WinUI, new []{ "txt", "hex", "TXT", "HEX"}}
            })
            
        });
        return fileResult;
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
}