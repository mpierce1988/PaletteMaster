using System.ComponentModel.DataAnnotations;
using PaletteMaster.Models.Domain;

namespace PaletteMaster.Models.DTO.ImageProcessing;

public class ProcessFolderRequest
{             
    [Required]
    [MinLength(1, ErrorMessage = "Source Folder is Required")]
    public string SourceFolder { get; set; }
    
    [Required]
    [MinLength(1, ErrorMessage = "Output Folder is Required")]
    public string OutputFolder { get; set; }

    [Required]
    [MinLength(1, ErrorMessage = "At Least One Color Is Required")]
    public List<Color>? Colors { get; set; } = new();
    
    public ProcessFolderRequest() {}

    public ProcessFolderRequest(string sourceFolder, string outputFolder, List<Color> colors)
    {
        SourceFolder = sourceFolder;
        OutputFolder = outputFolder;
        Colors = colors;
    }
}