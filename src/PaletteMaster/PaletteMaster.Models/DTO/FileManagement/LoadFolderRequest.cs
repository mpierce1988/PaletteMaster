using System.ComponentModel.DataAnnotations;

namespace PaletteMaster.Models.DTO.FileManagement;

public class LoadFolderRequest
{
    [Required]
    [MinLength(1, ErrorMessage = "Path cannot be an empty string")]
    public string Path { get; set; }
    
    public LoadFolderRequest(string path)
    {
        Path = path;
    }
}