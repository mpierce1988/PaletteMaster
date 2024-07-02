using System.ComponentModel.DataAnnotations;
using PaletteMaster.Models.Domain;

namespace PaletteMaster.Models.DTO.Palettes;

public class GetPalettesRequest
{
    public string? Name { get; set; }
    public List<Color>? Colors { get; set; }
    public GetPalettesSorting Sorting { get; set; } = GetPalettesSorting.NameAsc;
    
    [Range(1, int.MaxValue, ErrorMessage = "Page must be greater than 0")]
    public int Page { get; set; } = 1;
    
    [Range(1, int.MaxValue, ErrorMessage = "Page size must be greater than 0")]
    public int PageSize { get; set; } = 10;
}