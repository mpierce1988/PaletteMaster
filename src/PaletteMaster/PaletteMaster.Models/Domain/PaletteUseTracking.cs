using System.ComponentModel.DataAnnotations.Schema;

namespace PaletteMaster.Models.Domain;

public class PaletteUseTracking : BaseEntity
{
    public int PaletteUseTrackingId { get; set; }
    
    [ForeignKey("Palette")]
    public int PaletteId { get; set; }

    public virtual Palette Palette { get; set; } = default!;
    
    public PaletteUseTracking (int paletteId)
    {
        PaletteId = paletteId;
    }

    public PaletteUseTracking(Palette palette)
    {
        PaletteId = palette.PaletteId;
    }
}