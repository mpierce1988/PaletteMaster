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

    private async Task ApplyPaletteToImage(Image<Rgba32> image, List<Color> allowedColors)
    {
        await Task.Run(() =>
        {
            // Add a transparent color, so transparent pixels in the source image are converted to a transparent color in the output
            allowedColors.Add(new Color("#00000000"));
            ReadOnlyMemory<SixLabors.ImageSharp.Color> colors = allowedColors.Select(c => new SixLabors.ImageSharp.Color(c.ToVector4())).ToArray();
        
            image.Mutate(c => c.Quantize());
        });
    }
}