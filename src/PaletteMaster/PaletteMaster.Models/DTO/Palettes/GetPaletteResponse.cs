using PaletteMaster.Models.Domain;

namespace PaletteMaster.Models.DTO.Palettes;

public record GetPaletteResponse
{
    public int PaletteId { get; set; }
    public string Name { get; set; }
    public List<Color> Colors { get; set; } = new List<Color>();
    public List<PaletteUseTracking> PaletteUseTrackings { get; set; } = new List<PaletteUseTracking>();
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public DateTime? LastUsedDate => PaletteUseTrackings.MaxBy(tracking => tracking.CreatedDate)?.CreatedDate ?? null;

    public GetPaletteResponse(Palette palette)
    {
        
        PaletteId = palette.PaletteId;
        Name = palette.Name;
        Colors = palette.Colors;
        PaletteUseTrackings = palette.PaletteUseTrackings;
        CreatedDate = palette.CreatedDate;
        ModifiedDate = palette.ModifiedDate;
    }
}