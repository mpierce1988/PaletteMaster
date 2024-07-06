using System.Net.Mime;

namespace PaletteMaster.Models.DTO.ImageProcessing;

public class ImageProcessingResponse
{
    public Stream FileStream { get; set; }
    public string FileName { get; set; }
    
    public string FilePath { get; set; }

    public ImageProcessingResponse(Stream fileStream, string fileName)
    {
        FileStream = fileStream;
        FileName = fileName;
    }
}