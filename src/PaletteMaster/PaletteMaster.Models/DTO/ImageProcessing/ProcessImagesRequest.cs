using System.ComponentModel.DataAnnotations;
using PaletteMaster.Models.Domain;

namespace PaletteMaster.Models.DTO.ImageProcessing;

public class ProcessImagesRequest
{
    [Required]
    [MinLength(1, ErrorMessage = "At Least One Image Is Required")]
    public List<ProcessImageRequest> ImagesToProcess { get; set; } = new();
    
    [Required(ErrorMessage = "Colors Are Required")]
    [MinLength(1, ErrorMessage = "At Least One Color Is Required")]
    public List<Color> Colors { get; set; } = new();
}