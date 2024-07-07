namespace PaletteMaster.Models.DTO.FileManagement;

public class FileResponse
{
    public string FileName { get; set; }
    public string Path { get; set; }
    public FileStream FileStream { get; set; }
    
    public FileResponse(string fileName, string path, FileStream fileStream)
    {
        FileName = fileName;
        Path = path;
        FileStream = fileStream;
    }
}