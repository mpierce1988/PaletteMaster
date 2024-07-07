namespace PaletteMaster.Models.DTO.FileManagement;

public class FileToSave
{
    public string FileName { get; set; }
    public string RelativePath { get; set; }
    public MemoryStream Stream { get; set; }
    
    public FileToSave(string fileName, string relativePath, MemoryStream stream)
    {
        FileName = fileName;
        RelativePath = relativePath;
        Stream = stream;
    }
}