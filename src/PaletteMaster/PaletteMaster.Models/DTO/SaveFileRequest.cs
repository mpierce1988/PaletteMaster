using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace PaletteMaster.Models.DTO;

public class SaveFileRequest
{
    [Required]
    public MemoryStream? FileStream { get; set; }
    
    [Required]
    public string? FileName { get; set; }
}