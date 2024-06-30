using System.ComponentModel.DataAnnotations;

namespace PaletteMaster.Models.DTO.Palettes;

public class GetPaletteRequest
{
    [Required(ErrorMessage = "PalletId is required")]
    [Range(minimum: 1, maximum: int.MaxValue, ErrorMessage = "PalletId must be greater than 0")]
    public int PalletId { get; set; }
}