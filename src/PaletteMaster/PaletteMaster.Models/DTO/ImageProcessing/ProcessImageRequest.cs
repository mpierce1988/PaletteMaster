using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using PaletteMaster.Models.Domain;

namespace PaletteMaster.Models.DTO.ImageProcessing;

public class ProcessImageRequest
{
    [Required(ErrorMessage = "Colors Are Required")]
    [MinLength(1, ErrorMessage = "At Least One Color Is Required")]
    public List<Color> Colors { get; set; } = new();
    
    public Stream? FileStream { get; set; }
    
    [NotNull]
    [CustomValidation(typeof(ProcessImageRequest), "ValidateFileName")]
    public string? FileName { get; set; }
    
    public string? FilePath { get; set; }
    
    public string? RelativePath { get; set; }
    
    
    
    public static ValidationResult ValidateFileName(string? fileName, ValidationContext context)
    {
        if (fileName is null)
        {
            return new ValidationResult("File Name Is Required");
        }
        
        if (!fileName.EndsWith(".png") && !fileName.EndsWith(".jpg"))
        {
            return new ValidationResult("File Must Be A .png or .jpg File");
        }

        return ValidationResult.Success;
    }
}