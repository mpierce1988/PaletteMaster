using Moq;
using PaletteMaster.Models;
using PaletteMaster.Models.Domain;
using PaletteMaster.Models.DTO.Palettes;
using PaletteMaster.Services.Palettes;

namespace PaletteMaster.Services.Tests;

public class PaletteServiceTests
{
    [Fact]
    public async Task GetPalettesAsync_NoSearchCriteria_ReturnsAllPalettes()
    {
        // Arrange
        var palettes = new List<Palette>
        {
            new Palette { PaletteId = 1, Name = "Palette 1", Colors = new List<Color> { new Color { Hexadecimal = "#FF0000"} } },
            new Palette { PaletteId = 2, Name = "Palette 2", Colors = new List<Color> { new Color { Hexadecimal = "#00FF00"} } },
            new Palette { PaletteId = 3, Name = "Palette 3", Colors = new List<Color> { new Color { Hexadecimal = "#0000FF"} } }
        };

        
        
        // use moq to mock IPaletteRepository
        var mockRepository = new Mock<IPaletteRepository>();
        mockRepository.Setup(repo => repo.GetPalettesAsync(It.IsAny<GetPalettesRequest>()))
            .ReturnsAsync(palettes);
        
        var service = new PaletteService(mockRepository.Object);
        
        // Act
        var result = await service.GetPalettesAsync(new GetPalettesRequest());

        GetPalettesResponse? response = result.Match<GetPalettesResponse?>(
            success: response => response,
            failure: error => null
            );
        
        // Assert
        Assert.Equal(palettes.Count, response!.Palettes.Count);
    }
    
    // Page is 0 or less, returns HandledException
    [Fact]
    public async Task GetPalettesAsync_PageLessThan1_ReturnsHandledException()
    {
        // Arrange
        var service = new PaletteService(Mock.Of<IPaletteRepository>());
        
        // Act
        var result = await service.GetPalettesAsync(new GetPalettesRequest { Page = 0 });

        HandledException? exception = result.Match<HandledException?>(
            success: response => null,
            failure: error => error
            );
        
        // Assert
        Assert.NotNull(exception);
        Assert.True(exception.Value.IsValidationError);
    }
    
    // PageSize is 0 or less, returns HandledException
    [Fact]
    public async Task GetPalettesAsync_PageSizeLessThan1_ReturnsHandledException()
    {
        // Arrange
        var service = new PaletteService(Mock.Of<IPaletteRepository>());
        
        // Act
        var result = await service.GetPalettesAsync(new GetPalettesRequest { PageSize = 0 });

        HandledException? exception = result.Match<HandledException?>(
            success: response => null,
            failure: error => error
            );
        
        // Assert
        Assert.NotNull(exception);
        Assert.True(exception.Value.IsValidationError);
    }
    
    // Search for name - returns correct result
    // Search for color - returns correct result
    // results limited to page size
}