using PaletteMaster.Models.Domain;

namespace PaletteMaster.Models.DTO.Palettes;

public class CreatePaletteResponse
{
    public int PaletteId { get; set; }
    public string Name { get; set; } = default!;
    
    public List<Color> Colors { get; set; } = new List<Color>();
    
    public DateTime CreatedDate { get; set; }

    public CreatePaletteResponse()
    {
    }
    
    public CreatePaletteResponse(Palette palette)
    {
        PaletteId = palette.PaletteId;
        Name = palette.Name;
        Colors = palette.Colors;
        CreatedDate = palette.CreatedDate;
    }
}