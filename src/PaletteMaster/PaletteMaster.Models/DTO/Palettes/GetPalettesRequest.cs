using PaletteMaster.Models.Domain;

namespace PaletteMaster.Models.DTO.Palettes;

public class GetPalettesRequest
{
    public string? Name { get; set; }
    public List<Color>? Colors { get; set; }
    public GetPalettesSorting Sorting { get; set; } = GetPalettesSorting.NameAsc;
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}