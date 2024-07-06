using System.ComponentModel.DataAnnotations;
using System.Numerics;
using System.Runtime.CompilerServices;
using PaletteMaster.Models;
using PaletteMaster.Models.DTO.ImageProcessing;
using PaletteMaster.Services.ImageProcessing;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Processors.Quantization;
using Color = PaletteMaster.Models.Domain.Color;

namespace PaletteMaster.Services.ImageSharp;

public class ImageSharpImageProcessingService : IImageProcessingService
{
    public async Task<Result<ImageProcessingResponse, HandledException>> ProcessImageAsync(ImageProcessingRequest request)
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

            // Convert stream to Image<Rgba32>
            //Image<Rgba32> image = Image.Load<Rgba32>(request.PathToImage);
            request.FileStream!.Position = 0;
            Image<Rgba32> image = SixLabors.ImageSharp.Image.Load<Rgba32>(request.FileStream);


            // Process Image
            await ApplyPaletteToImage(image, request.Colors);

            // Convert image back to stream
            MemoryStream stream = new();
            await image.SaveAsPngAsync(stream);
            stream.Position = 0;
            
            return new ImageProcessingResponse(stream, request.FileName);
        }
        catch (Exception e)
        {
            return new HandledException(e.Message);
        }
    }

    private async Task ApplyPaletteToImage(Image<Rgba32> image, List<Color> requestColors)
    {
        await Task.Run(() =>
        {
            image.Mutate(c => c.ProcessPixelRowsAsVector4(row =>
            {
                Rgba32 transparent = SixLabors.ImageSharp.Color.Transparent;
                foreach (ref Vector4 pixel in row)
                {
                    if (pixel.W == 0)
                    {
                        pixel = transparent.ToVector4();
                        continue;
                    }

                    Color pixelColor = new Color(pixel);
                    Color matchingColor = ImageProcessingUtility.MatchColor(pixelColor, requestColors);
                    pixel = matchingColor.ToVector4();
                }
            }, PixelConversionModifiers.Premultiply));
        });
    }
}