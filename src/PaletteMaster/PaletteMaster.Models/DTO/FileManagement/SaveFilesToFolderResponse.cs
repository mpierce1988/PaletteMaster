namespace PaletteMaster.Models.DTO.FileManagement;

public class SaveFilesToFolderResponse
{
    public string Path { get; set; }
    public int TotalFilesSaved { get; set; }
    
    public SaveFilesToFolderResponse(string path, int totalFilesSaved)
    {
        Path = path;
        TotalFilesSaved = totalFilesSaved;
    }
}