using System.ComponentModel.DataAnnotations;
using System.Numerics;
using PaletteMaster.Models;
using PaletteMaster.Models.DTO.ImageProcessing;
using PaletteMaster.Services.ImageProcessing;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Processors.Quantization;
using Color = PaletteMaster.Models.Domain.Color;

namespace PaletteMaster.Services.ImageSharp;

public class ImageSharpImageProcessingService : IImageProcessingService
{
    /// <summary>
    /// Processes an image according to the provided request parameters. This includes validating the request,
    /// converting the input stream to an Image<Rgba32>, applying a color palette transformation, and returning
    /// the processed image as a stream within an ImageProcessingResponse object.
    /// </summary>
    /// <param name="request">The image processing request containing the image stream and processing parameters.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the ImageProcessingResponse
    /// object with the processed image stream, or a HandledException detailing any errors encountered during processing.</returns>
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
            Image<Rgba32> image = Image.Load<Rgba32>(request.FileStream);

            // Process Image
            await ApplyPaletteToImageAsync(image, request.Colors);

            // Convert image back to stream
            MemoryStream stream = new();
            await image.SaveAsPngAsync(stream);
            stream.Position = 0;
            
            return new ImageProcessingResponse(stream, request.FileName, request.RelativePath!);
        }
        catch (Exception e)
        {
            return new HandledException(e.Message);
        }
    }

    public async Task<Result<ImageFolderProcessingResponse, HandledException>> ProcessImageFolderAsync(ImageFolderProcessingRequest request)
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

            // Process each image in the folder
            List<ImageProcessingResponse> imageResponses = new();
            
            foreach (ImageProcessingRequest imageRequest in request.ImagesToProcess)
            {
                Result<ImageProcessingResponse, HandledException> result = await ProcessImageAsync(imageRequest);

                HandledException? exception = result.Match<HandledException?>(
                    success: response =>
                    {
                        imageResponses.Add(response);
                        return null;
                    },
                    failure: error => error
                );

                if (exception is not null)
                {
                    return exception.Value;
                }
            }

            return new ImageFolderProcessingResponse(imageResponses);
        }
        catch (Exception e)
        {
            return new HandledException(e.Message);
        }
    }

    private async Task ApplyPaletteToImageAsync(Image<Rgba32> image, List<Color> allowedColors)
    {
        // Run the image processing on a background thread to prevent locking the main (UI) thread
        await Task.Run(() =>
        {
            PreprocessAlpha(image);
            
            ReadOnlyMemory<SixLabors.ImageSharp.Color> paletteColors = GetPaletteColors(allowedColors);
            QuantizerOptions quantizerOptions = new()
            {
                Dither = null
            };
            PaletteQuantizer quantizer = new(paletteColors, quantizerOptions);
            
            // Apply the palette to the image
            image.Mutate(x => x.Quantize(quantizer));
        });
    }

    /// <summary>
    /// Sets any pixel with an alpha value of 0 to a fully transparent color.
    /// </summary>
    /// <param name="image">Image to process</param>
    private void PreprocessAlpha(Image<Rgba32> image)
    {
        image.Mutate(c => c.ProcessPixelRowsAsVector4(row =>
        {
            for (int x = 0; x < row.Length; x++)
            {
                if (row[x].W == 0)
                    row[x] = Vector4.Zero;
            }
        }, PixelConversionModifiers.Premultiply));
    }

    /// <summary>
    /// Creates a ReadOnlyMemoty of SixLabors.ImageSharp.Color from a list of Color objects, including a transparent color.
    /// </summary>
    /// <param name="allowedColors">Colors to add to the palette</param>
    /// <returns>ReadOnlyMemory representing palette colors</returns>
    private ReadOnlyMemory<SixLabors.ImageSharp.Color> GetPaletteColors(List<Color> allowedColors)
    {
        // Add a transparent color to match transparent pixels in the image
        allowedColors.Add(new Color("#00000000"));
        
        return allowedColors.Select(c => new SixLabors.ImageSharp.Color(c.ToVector4())).ToArray();
    }
}