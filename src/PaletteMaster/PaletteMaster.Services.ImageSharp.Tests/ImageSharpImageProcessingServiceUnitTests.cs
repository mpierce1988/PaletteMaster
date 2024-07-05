using System.Runtime.CompilerServices;
using PaletteMaster.Models;
using PaletteMaster.Models.DTO.ImageProcessing;
using PaletteMaster.Services.ImageProcessing;
using PaletteMaster.Services.ImageSharp.Tests.Utilities;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Color = PaletteMaster.Models.Domain.Color;

namespace PaletteMaster.Services.ImageSharp.Tests;

public class ImageSharpImageProcessingServiceUnitTests
{
    private readonly IImageProcessingService _imageProcessingService = new ImageSharpImageProcessingService();

    private const string SangriaFileName = "sangria-8x.png";
    private const string MacPaintFileName = "mac-paint-8x.png";
    
    [Fact]
    public async Task Test1()
    {
        // Arrange
        await using Stream inputStream = File.OpenRead(TestUtility.GetSamplePath(MacPaintFileName));
        List<Color> colors = new()
        {
            new("#8bc8feff"),
            new("#051b2cff")
        };

        ImageProcessingRequest request = new()
        {
            PathToImage = TestUtility.GetSamplePath(MacPaintFileName),
            FileName = SangriaFileName,
            FileStream = inputStream,
            Colors = colors
        };

        Image<Rgba32> expectedImage = Image.Load<Rgba32>(inputStream);
        byte[] expectedBytes = new byte[expectedImage.Width * expectedImage.Height * Unsafe.SizeOf<Rgba32>()];
        expectedImage.CopyPixelDataTo(expectedBytes);

        // Act
        Result<ImageProcessingResponse, HandledException> result =
            await _imageProcessingService.ProcessImageAsync(request);

        ImageProcessingResponse? response = result.Match<ImageProcessingResponse?>(
            success: response =>
            {
                // Save the image to disk for manual inspection
                using FileStream fileStream = new(TestUtility.GetSamplePath("mac-paint-8x-palette.png"), FileMode.Create);
                fileStream.Write(response.FileStream, 0, response.FileStream.Length);
                fileStream.Flush();
                
                return response;
            },
            failure: error => null
        );
        
        Assert.NotNull(response);
        
        using Image<Rgba32> actualImage = Image.Load<Rgba32>(response.FileStream);
        
        Assert.True(AllColorsMatch(actualImage, colors));
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