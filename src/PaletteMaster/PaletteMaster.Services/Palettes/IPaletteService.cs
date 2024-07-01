using PaletteMaster.Models;
using PaletteMaster.Models.DTO.Palettes;

namespace PaletteMaster.Services.Palettes;

public interface IPaletteService
{
    public Task<Result<GetPalettesResponse, HandledException>> GetPalettesAsync(GetPalettesRequest request);
    public Task<Result<GetPaletteResponse, HandledException>> GetPaletteAsync(GetPaletteRequest request);
    public Task<Result<CreatePaletteResponse, HandledException>> CreatePaletteAsync(CreatePaletteRequest request);
}