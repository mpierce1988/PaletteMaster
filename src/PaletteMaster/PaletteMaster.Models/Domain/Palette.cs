using System.ComponentModel.DataAnnotations;

namespace PaletteMaster.Models.Domain;

public class Palette : BaseEntity
{
    public int PaletteId { get; set; }
    [Required(ErrorMessage = "Name is required")]
    [MinLength(3, ErrorMessage = "Name must be at least 3 characters long")]
    [MaxLength(50, ErrorMessage = "Name must be at most 50 characters long")]
    public string Name { get; set; }
    public List<Color> Colors { get; set; } = new List<Color>();
    
    public virtual List<PaletteUseTracking> PaletteUseTrackings { get; set; } = new List<PaletteUseTracking>();

    public Palette()
    {
        
    }

    public Palette(string name)
    {
        Name = name;
    }

    public Palette(string name, List<Color> colors)
    {
        Name = name;
        Colors = colors;
    }
}