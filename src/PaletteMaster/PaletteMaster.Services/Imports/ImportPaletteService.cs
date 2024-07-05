using System.ComponentModel.DataAnnotations;
using PaletteMaster.Models;
using PaletteMaster.Models.Domain;
using PaletteMaster.Models.DTO.Palettes;
using PaletteMaster.Services.Palettes;

namespace PaletteMaster.Services.Imports;

public class ImportPaletteService : IImportPaletteService
{
    public async Task<Result<ImportPaletteResponse, HandledException>> ImportPaletteAsync(ImportPaletteRequest request)
    {
        try
        {
            // Validate request parameters
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(request, null, null);

            if (!Validator.TryValidateObject(request, validationContext, validationResults, true))
            {
                return new HandledException(validationResults);
            }
            
            // Get File Type
            PaletteFileType fileType = GetFileType(request.Name);
            
            // Call the appropriate importer for the File Type
            List<Color> colors = fileType switch
            {
                PaletteFileType.Hex => ImportHexPalette(request.File),
                PaletteFileType.PaintNET => ImportPaintNetPalette(request.File),
                _ => throw new NotImplementedException()
            };

            // Return the result
            return new ImportPaletteResponse()
            {
                Name = request.Name,
                Colors = colors,
                FileType = fileType
            };
        }
        catch (Exception e)
        {
            return new HandledException(e.Message);
        }
    }

    private List<Color> ImportPaintNetPalette(Stream requestFile)
    {
        // Parse the requestFile stream to text
        using StreamReader reader = new(requestFile);
        string text = reader.ReadToEnd();
        
        // Filter out comments
        string[] lines = text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries)
            .Where(line => !line.StartsWith(";"))
            .ToArray();
        
        // Parse each line into a Color, removing the first two characters ('FF')
        List<Color> colors = lines.Select(hex => new Color(hex.Substring(2))).ToList();

        return colors;
    }

    private List<Color> ImportHexPalette(Stream requestFile)
    {
        // Parse requestFile stream to text
        using StreamReader reader = new(requestFile);
        string text = reader.ReadToEnd();
        
        // Split the text into lines
        string[] lines = text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
        
        // Parse each line into a Color
        List<Color> colors = lines.Select(hex => new Color(hex)).ToList();
        
        return colors;
    }

    private PaletteFileType GetFileType(string requestName)
    {
        // Get the file extension
        string extension = Path.GetExtension(requestName);

        return extension switch
        {
            ".hex" => PaletteFileType.Hex,
            ".txt" => PaletteFileType.PaintNET,
            _ => throw new NotImplementedException($"The Extension {extension} is not supported.")
        };
    }
}