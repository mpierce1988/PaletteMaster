using System.ComponentModel.DataAnnotations;
using PaletteMaster.Models.Domain;

namespace PaletteMaster.Models.DTO.Palettes;

public class CreatePaletteRequest
{
    [Required(ErrorMessage = "Name is required")]
    [MinLength(3, ErrorMessage = "Name must be at least 3 characters long")]
    [MaxLength(50, ErrorMessage = "Name must be at most 50 characters long")]
    public string Name { get; set; } = default!;
    
    [MinLength(1, ErrorMessage = "At least one color is required")]
    public List<Color> Colors { get; set; } = new List<Color>();
}