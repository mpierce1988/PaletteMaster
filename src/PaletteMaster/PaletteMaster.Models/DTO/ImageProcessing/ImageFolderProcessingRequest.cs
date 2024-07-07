using System.ComponentModel.DataAnnotations;
using PaletteMaster.Models.Domain;

namespace PaletteMaster.Models.DTO.ImageProcessing;

public class ImageFolderProcessingRequest
{
    [Required]
    [MinLength(1, ErrorMessage = "At Least One Image Is Required")]
    public List<ImageProcessingRequest> ImagesToProcess { get; set; } = new();
    
    [Required(ErrorMessage = "Colors Are Required")]
    [MinLength(1, ErrorMessage = "At Least One Color Is Required")]
    public List<Color> Colors { get; set; } = new();
}