using PaletteMaster.Models.Domain;
using PaletteMaster.Models.DTO.Palettes;

namespace PaletteMaster.Services.Palettes;

public interface IPaletteRepository
{
    Task<List<Palette>> GetPalettesAsync(GetPalettesRequest request);
    Task<int> GetPalettesCountAsync(GetPalettesRequest request);
    Task<Palette?> GetPaletteAsync(int requestPalletId);
    Task<Palette?> GetPaletteWithUseTrackingAsync(int requestPalleteId);
    Task<Palette> CreatePaletteAsync(Palette palette);
    Task<Palette> UpdatePaletteAsync(Palette palette);
    Task DeletePaletteAsync(int paletteId);
}