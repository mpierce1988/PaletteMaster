using System.Runtime.CompilerServices;
using PaletteMaster.Models;
using PaletteMaster.Models.DTO.FileManagement;
using PaletteMaster.Models.DTO.ImageProcessing;
using PaletteMaster.Services.FileManagement;
using PaletteMaster.Services.ImageProcessing;
using PaletteMaster.Services.ImageSharp.Tests.Utilities;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Color = PaletteMaster.Models.Domain.Color;

namespace PaletteMaster.Services.ImageSharp.Tests;

public class ImageSharpImageProcessingServiceUnitTests
{
    private readonly IImageProcessingService _imageProcessingService = new ImageSharpImageProcessingService(new FileManagementService());
    private readonly IFileManagementService _fileManagementService = new FileManagementService();

    private const string SangriaFileName = "sangria-8x.png";
    private const string MacPaintFileName = "mac-paint-8x.png";
    private const string OrcFileName = "orc07.png";
    private const string ThreeItemFolder = "ThreeItemFolder";
    
    [Fact]
    public async Task ProcessImageAsync_OriginalImageHasTwoPaletteColors_ResultIdenticalToOriginal()
    {
        // Arrange
        await using Stream inputStream = File.OpenRead(TestUtility.GetSamplePath(MacPaintFileName));
        List<Color> colors = new()
        {
            new("#8bc8feff"),
            new("#051b2cff")
        };

        ProcessImageRequest request = new()
        {
            FileName = SangriaFileName,
            FileStream = inputStream,
            Colors = colors
        };

        Image<Rgba32> expectedImage = Image.Load<Rgba32>(inputStream);
        byte[] expectedBytes = new byte[expectedImage.Width * expectedImage.Height * Unsafe.SizeOf<Rgba32>()];
        expectedImage.CopyPixelDataTo(expectedBytes);

        // Act
        Result<ProcessImageResponse, HandledException> result =
            await _imageProcessingService.ProcessImageAsync(request);

        ProcessImageResponse? response = result.Match<ProcessImageResponse?>(
            success: response =>
            {
                // Save the image to disk for manual inspection
                using FileStream fileStream = new(TestUtility.GetSamplePath("mac-paint-8x-palette.png"), FileMode.Create);
                MemoryStream memoryStream = (MemoryStream)response.Stream;
                memoryStream.Position = 0;
                byte[] imageBytes = memoryStream.ToArray();
                fileStream.Write(imageBytes, 0, imageBytes.Length);
                fileStream.Flush();
                
                return response;
            },
            failure: error => null
        );
        
        Assert.NotNull(response);
        
        using Image<Rgba32> actualImage = Image.Load<Rgba32>(response.Stream);
        
        Assert.True(AllColorsMatch(actualImage, colors));
    }
    
    [Fact]
    public async Task ProcessImageAsync_OriginalImageHasTransparency_ResultHasCorrectColorsTransparency()
    {
        // Arrange
        await using Stream inputStream = File.OpenRead(TestUtility.GetSamplePath(OrcFileName));
        List<Color> colors = new()
        {
            new("#8bc8feff"),
            new("#051b2cff")
        };

        ProcessImageRequest request = new()
        {
            FileName = SangriaFileName,
            FileStream = inputStream,
            Colors = colors
        };

        Image<Rgba32> inputImage = Image.Load<Rgba32>(inputStream);
        byte[] expectedBytes = new byte[inputImage.Width * inputImage.Height * Unsafe.SizeOf<Rgba32>()];
        inputImage.CopyPixelDataTo(expectedBytes);

        // Act
        Result<ProcessImageResponse, HandledException> result =
            await _imageProcessingService.ProcessImageAsync(request);

        ProcessImageResponse? response = result.Match<ProcessImageResponse?>(
            success: response =>
            {
                // Save the image to disk for manual inspection
                using FileStream fileStream = new(TestUtility.GetSamplePath($"{OrcFileName.Split('.')[0]}-palette.png"), FileMode.Create);
                MemoryStream memoryStream = response.Stream;
                memoryStream.Position = 0;
                byte[] imageBytes = memoryStream.ToArray();
                fileStream.Write(imageBytes, 0, imageBytes.Length);
                fileStream.Flush();
                
                return response;
            },
            failure: error => null
        );
        
        Assert.NotNull(response);
        
        using Image<Rgba32> resultImage = Image.Load<Rgba32>(response.Stream);
        
        Assert.True(AllTransparencyPreserved(inputImage, resultImage));
        Assert.True(AllColorsMatch(resultImage, colors));
    }

    [Fact]
    public async Task ProcessImageFolderAsync_ThreeItems_ResultHasThreeItems()
    {
        // Arrange
        string fullPath = TestUtility.GetSamplePath(ThreeItemFolder);
        LoadFolderRequest folderRequest = new(fullPath);
        var folderResult = await _fileManagementService.LoadFolderAsync(folderRequest);
        
        var (folderResponse, folderError) = folderResult.Match<(LoadFolderResponse?, HandledException?)>(
            success: response => (response, null),
            failure: error => (null, error)
        );
        
        Assert.NotNull(folderResponse);
        
        List<Color> colors = new()
        {
            new("#8bc8feff"),
            new("#051b2cff")
        };
        
        var imageFolderProcessingRequest = folderResponse.ToImageFolderProcessingRequest(colors);
        
        // Act
        Result<ProcessImagesResponse, HandledException> result =
            await _imageProcessingService.ProcessImagesAsync(imageFolderProcessingRequest);

        var (response, error) = result.Match<(ProcessImagesResponse?, HandledException?)>(
            success: response => (response, null),
            failure: error => (null, error)
            );
        
        // Assert
        Assert.NotNull(response);
        Assert.Equal(3, response.ProcessedImages.Count);
    }

    [Fact]
    public async Task ProcessImageFolderAsync_NoImages_ReturnsHandledException()
    {
        // Arrange
        ProcessImagesRequest request = new();
        List<Color> colors = new()
        {
            new("#8bc8feff"),
            new("#051b2cff")
        };

        request.Colors = colors;
        
        // Act
        Result<ProcessImagesResponse, HandledException> result =
            await _imageProcessingService.ProcessImagesAsync(request);

        HandledException? exception = result.Match<HandledException?>(
            success: response => null,
            failure: error => error
            );
        
        // Assert
        Assert.NotNull(exception);
    }
    
    [Fact]
    public async Task ProcessImageFolderAsync_NoColors_ReturnsHandledException()
    {
        // Arrange
        string fullPath = TestUtility.GetSamplePath(ThreeItemFolder);
        LoadFolderRequest folderRequest = new(fullPath);
        var folderResult = await _fileManagementService.LoadFolderAsync(folderRequest);
        
        var (folderResponse, folderError) = folderResult.Match<(LoadFolderResponse?, HandledException?)>(
            success: resp => (resp, null),
            failure: err => (null, err)
        );
        
        Assert.NotNull(folderResponse);
        
        var imageFolderProcessingRequest = folderResponse.ToImageFolderProcessingRequest();
        
        // Act
        Result<ProcessImagesResponse, HandledException> result =
            await _imageProcessingService.ProcessImagesAsync(imageFolderProcessingRequest);

        var (response, error) = result.Match<(ProcessImagesResponse?, HandledException?)>(
            success: response => (response, null),
            failure: error => (null, error)
        );
        
        // Assert
        Assert.NotNull(error);
    }

    private bool AllTransparencyPreserved(Image<Rgba32> originalImage, Image<Rgba32> resultImage)
    {
        bool allTransparenciesMatch = true;

        originalImage.ProcessPixelRows(accessor =>
        {
            for (int y = 0; y < accessor.Height; y++)
            {
                Span<Rgba32> originalPixelRow = accessor.GetRowSpan(y);
                Span<Rgba32> resultPixelRow = accessor.GetRowSpan(y);

                for (int x = 0; x < originalPixelRow.Length; x++)
                {
                    if (originalPixelRow[x].A != 0) continue;
                    
                    // Original pixel is transparent, so the result pixel should also be transparent
                    if (resultPixelRow[x].A != 0)
                    {
                        allTransparenciesMatch = false;
                        break;
                    }
                }


            }
        });
        return allTransparenciesMatch;
    }

    private bool AllColorsMatch(Image<Rgba32> image, List<Color> expectedColors)
    {
        bool allColorsMatch = true;

        image.ProcessPixelRows(accessor =>
        {
            for (int y = 0; y < accessor.Height; y++)
            {
                Span<Rgba32> pixelRow = accessor.GetRowSpan(y);

                foreach (var pixel in pixelRow)
                {
                    Color actualPixel = new(pixel.ToHex());
                    // If the actual pixel matches an expected color, go to the next pixel
                    if (expectedColors.Any(expectedColor => string.Equals(expectedColor.Hexadecimal, actualPixel.Hexadecimal, StringComparison.CurrentCultureIgnoreCase))) continue;
                    // If the actual pixel does not match any expected color, set allColorsMatch to false and break
                    allColorsMatch = false;
                    break;
                }

                // If we've already detected an unexpected color, stop processing rows and break out of this loop
                if (!allColorsMatch) break;
            }
        });
        
        return allColorsMatch;
    }
}