﻿using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using PaletteMaster.Models;
using PaletteMaster.Models.DTO.ImageProcessing;
using PaletteMaster.Services.ImageProcessing;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
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
            Image<Rgba32> image = Image.Load<Rgba32>(request.PathToImage);
            //Image image = SixLabors.ImageSharp.Image.Load(request.FileStream);


            // Process Image
            ApplyPaletteToImage(image!, request.Colors);

            // Convert image back to stream
            MemoryStream stream = new();
            await image.SaveAsPngAsync(stream);
            stream.Position = 0;

            if (stream.TryGetBuffer(out var buffer))
            {
                return new ImageProcessingResponse(buffer.Array!, request.FileName);
            }

            return new HandledException("Failed to convert image to stream");
            
        }
        catch (Exception e)
        {
            return new HandledException(e.Message);
        }
    }

    private void ApplyPaletteToImage(Image<Rgba32> image, List<Color> requestColors)
    {
        image.ProcessPixelRows(accessor =>
        {
            for (int y = 0; y < accessor.Height; y++)
            {
                Span<Rgba32> pixelRow = accessor.GetRowSpan(y);

                for (int x = 0; x < pixelRow.Length; x++)
                {
                    // Get a refrerence to the pixel at this location
                    ref Rgba32 pixel = ref pixelRow[x];
                    Color matchingColor = ImageProcessingUtility.MatchColor(new Color(pixel.ToHex()), requestColors);
                    pixel = Rgba32.ParseHex(matchingColor.Hexadecimal);
                }
            }
        });
    }
}