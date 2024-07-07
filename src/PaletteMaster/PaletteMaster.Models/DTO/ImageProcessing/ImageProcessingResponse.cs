using System.Net.Mime;

namespace PaletteMaster.Models.DTO.ImageProcessing;

public class ImageProcessingResponse
{
    public MemoryStream Stream { get; set; }
    public string FileName { get; set; }
    public string RelativePath { get; set; }

    public ImageProcessingResponse(MemoryStream stream, string fileName, string relativePath)
    {
        Stream = stream;
        FileName = fileName;
        RelativePath = relativePath;
    }
}