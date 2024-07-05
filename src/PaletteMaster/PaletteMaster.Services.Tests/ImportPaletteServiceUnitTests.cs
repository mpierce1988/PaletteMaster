using PaletteMaster.Models;
using PaletteMaster.Models.Domain;
using PaletteMaster.Models.DTO.Palettes;
using PaletteMaster.Services.Imports;
using PaletteMaster.Services.Palettes;
using PaletteMaster.Services.Tests.Utilities;

namespace PaletteMaster.Services.Tests;

public class ImportPaletteServiceUnitTests
{
    private readonly string PaintNETSamplePath = TestUtility.GetSamplePath("PaintNETSample.txt");
    private readonly string HexSamplePath = TestUtility.GetSamplePath("HexSample.hex");
    private IImportPaletteService _importPaletteService = new ImportPaletteService();

    [Fact]
    public async Task ImportPalette_ImportPaintNET_CorrectResult()
    {
        // Arrange
        await using Stream fileStream = File.OpenRead(PaintNETSamplePath);
        
        ImportPaletteRequest request = new ImportPaletteRequest
        {
            File = fileStream,
            Name = "PaintNetSample.txt"
        };

        List<Color> expectedColors = new()
        {
            new Color("#47009f"),
            new Color("#5f73ff"),
            new Color("#ace7ff"),
            new Color("#ffffff")
        };
        
        // Act
        Result<ImportPaletteResponse, HandledException> result = await _importPaletteService.ImportPaletteAsync(request);

        ImportPaletteResponse? results = result.Match<ImportPaletteResponse?>(
            success: response => response,
            failure: error => null
        );
        
        // Assert
        Assert.NotNull(results);
        Assert.NotNull(results.Colors);
        Assert.Equal(expectedColors.Count, results.Colors!.Count);

        foreach (Color expectedColor in expectedColors)
        {
            Assert.Contains(expectedColor, results.Colors);
        }
    }

    [Fact]
    public async Task ImportPaletteAsync_ImportHexFile_CorrectResults()
    {
        // Arrange
        await using Stream fileStream = File.OpenRead(HexSamplePath);
        
        ImportPaletteRequest request = new ImportPaletteRequest
        {
            File = fileStream,
            Name = "HexSample.hex"
        };

        List<Color> expectedColors = new()
        {
            new Color("#47009f"),
            new Color("#5f73ff"),
            new Color("#ace7ff"),
            new Color("#ffffff")
        };
        
        // Act
        Result<ImportPaletteResponse, HandledException> result = await _importPaletteService.ImportPaletteAsync(request);

        ImportPaletteResponse? results = result.Match<ImportPaletteResponse?>(
            success: response => response,
            failure: error => null
        );
        
        // Assert
        Assert.NotNull(results);
        Assert.NotNull(results.Colors);
        Assert.Equal(expectedColors.Count, results.Colors!.Count);

        foreach (Color expectedColor in expectedColors)
        {
            Assert.Contains(expectedColor, results.Colors);
        }
    }

}