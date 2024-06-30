using PaletteMaster.Models.Domain;

namespace PaletteMaster.Models.DTO.Palettes;

public class CreatePaletteResponse
{
    public string Name { get; set; } = default!;
    
    public List<Color> Colors { get; set; } = new List<Color>();
    
    public DateTime CreatedDate { get; set; }
}