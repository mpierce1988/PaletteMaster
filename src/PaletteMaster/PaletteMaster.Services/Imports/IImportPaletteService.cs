using PaletteMaster.Models;
using PaletteMaster.Models.DTO.Palettes;

namespace PaletteMaster.Services.Palettes;

public interface IImportPaletteService
{
    public Task<Result<ImportPaletteResponse, HandledException>> ImportPaletteAsync(ImportPaletteRequest request);
}