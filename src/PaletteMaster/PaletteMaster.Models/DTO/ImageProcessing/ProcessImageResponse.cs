using System.Net.Mime;

namespace PaletteMaster.Models.DTO.ImageProcessing;

public class ProcessImageResponse
{
    public MemoryStream Stream { get; set; }
    public string FileName { get; set; }
    public string RelativePath { get; set; }

    public ProcessImageResponse(MemoryStream stream, string fileName, string relativePath)
    {
        Stream = stream;
        FileName = fileName;
        RelativePath = relativePath;
    }
}