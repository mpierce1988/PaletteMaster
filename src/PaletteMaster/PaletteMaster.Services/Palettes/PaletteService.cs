using System.ComponentModel.DataAnnotations;
using PaletteMaster.Models;
using PaletteMaster.Models.Domain;
using PaletteMaster.Models.DTO.Palettes;

namespace PaletteMaster.Services.Palettes;

public class PaletteService : IPaletteService
{
    private readonly IPaletteRepository _paletteRepository;

    public PaletteService(IPaletteRepository paletteRepository)
    {
        _paletteRepository = paletteRepository;
    }
    
    public async Task<Result<GetPalettesResponse, HandledException>> GetPalettesAsync(GetPalettesRequest request)
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

            // Get Palettes from repository, limited to the page size
            var palettes = await _paletteRepository.GetPalettesAsync(request);
            // Get total count of palettes that match this search criteria
            int totalCount = palettes.Count;

            if (totalCount == request.PageSize)
            {
                totalCount = await _paletteRepository.GetPalettesCountAsync(request);
            }

            // Map to response
            GetPalettesResponse response = new();
            response.Palettes = palettes;
            response.TotalCount = totalCount;
            response.Sorting = request.Sorting;
            response.Page = request.Page;
            response.PageSize = request.PageSize;

            return response;
        }
        catch (Exception e)
        {
            return new HandledException(e.Message);
        }
    }

    public async Task<Result<GetPaletteResponse, HandledException>> GetPaletteAsync(GetPaletteRequest request)
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

            // Get Palette by ID from repository
            Palette? palette = request.IncludeUseTracking
                ? await _paletteRepository.GetPaletteWithUseTrackingAsync(request.PalleteId)
                : await _paletteRepository.GetPaletteAsync(request.PalleteId);

            if (palette is null) return new HandledException("Palette Not Found");

            return new GetPaletteResponse(palette);
        }
        catch (Exception e)
        {
            return new HandledException(e.Message);
        }
    }

    public async Task<Result<CreatePaletteResponse, HandledException>> CreatePaletteAsync(CreatePaletteRequest request)
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

            // Create Palette from request
            Palette palette = new(request.Name, request.Colors);

            // Save Palette to repository
            palette = await _paletteRepository.CreatePaletteAsync(palette);

            return new CreatePaletteResponse(palette);
        }
        catch (Exception e)
        {
            return new HandledException(e.Message);
        }

    }

    public async Task<Result<UpdatePaletteResponse, HandledException>> UpdatePaletteAsync(UpdatePaletteRequest request)
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
            
                // Get Palette by ID from repository
                Palette? palette = await _paletteRepository.GetPaletteAsync(request.PaletteId);
    
                if (palette is null) return new HandledException("Palette Not Found");
    
                // Update Palette from request
                palette.Name = request.Name;
                palette.Colors = request.Colors;
    
                // Save Palette to repository
                palette = await _paletteRepository.UpdatePaletteAsync(palette);
    
                return new UpdatePaletteResponse(palette);
        }
        catch (Exception e)
        {
            return new HandledException(e.Message);
        }
    }

    public async Task<Result<bool, HandledException>> DeletePaletteAsync(int paletteId)
    {
        try
        {
            await _paletteRepository.DeletePaletteAsync(paletteId);

            return true;
        }
        catch (Exception e)
        {
            return new HandledException(e.Message);
        }
    }
}