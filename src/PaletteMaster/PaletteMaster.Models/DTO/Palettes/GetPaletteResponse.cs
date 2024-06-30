using PaletteMaster.Models.Domain;

namespace PaletteMaster.Models.DTO.Palettes;

public record GetPaletteResponse
{
    public int PaletteId { get; set; }
    public string Name { get; set; }
    public List<Color> Colors { get; set; } = new List<Color>();
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public DateTime? LastUsedDate { get; set; }

    public GetPaletteResponse(Palette palette)
    {
        
        PaletteId = palette.PaletteId;
        Name = palette.Name;
        Colors = palette.Colors;
        CreatedDate = palette.CreatedDate;
        ModifiedDate = palette.ModifiedDate;
        if (palette.PaletteUseTrackings.Count > 0)
        {
            LastUsedDate = palette.PaletteUseTrackings.MaxBy(tracking => tracking.CreatedDate)!.CreatedDate;
        }
    }
}