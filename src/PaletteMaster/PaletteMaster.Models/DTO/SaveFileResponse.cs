namespace PaletteMaster.Models.DTO;

public class SaveFileResponse
{
    public string FileName { get; set; }
    public string FilePath { get; set; }
    
    public SaveFileResponse(string fileName, string filePath)
    {
        FileName = fileName;
        FilePath = filePath;
    }
}