using System.ComponentModel.DataAnnotations;

namespace PaletteMaster.Models.DTO.Palettes;

public class UpdatePaletteRequest : CreatePaletteRequest
{
    [Required(ErrorMessage = "PaletteId is required")]
    [Range(1, int.MaxValue, ErrorMessage = "PaletteId must be greater than 0")]
    public int PaletteId { get; set; }
}