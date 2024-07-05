using PaletteMaster.Models.Domain;

namespace PaletteMaster.Models.DTO.Palettes;

public class ImportPaletteResponse
{
    public List<Color>? Colors { get; set; }
    public PaletteFileType FileType { get; set; }
    public string Name { get; set; } = default!;
}