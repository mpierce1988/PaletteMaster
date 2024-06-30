using PaletteMaster.Models.Domain;

namespace PaletteMaster.Models.DTO.Palettes;

public class GetPalettesResponse
{
    public int Count { get; set; }
    public List<Palette> Palettes { get; set; } = new List<Palette>();
    public GetPalettesSorting Sorting { get; set; } = GetPalettesSorting.NameAsc;
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public bool HasPreviousPage => Page > 1;
    public bool HasNextPage => Page < (Count / PageSize) + 1;
}