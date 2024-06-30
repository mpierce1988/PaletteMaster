using System.ComponentModel.DataAnnotations;

namespace PaletteMaster.Models.DTO.Palettes;

public class ImportPaletteRequest
{
    [Required(ErrorMessage = "File is required")]
    public Stream File { get; set; } = default!;
    
    [Required(ErrorMessage = "FileType is required")]
    public PaletteFileType FileType { get; set; }

    [MinLength(3, ErrorMessage = "Name must be at least 3 characters long")]
    public string Name { get; set; } = default!;
}