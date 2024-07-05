using System.Net.Mime;

namespace PaletteMaster.Models.DTO.ImageProcessing;

public class ImageProcessingResponse
{
    public byte[] FileStream { get; set; }
    public string FileName { get; set; }

    public ImageProcessingResponse(byte[] fileStream, string fileName)
    {
        FileStream = fileStream;
        FileName = fileName;
    }
}