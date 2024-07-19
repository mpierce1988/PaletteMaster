namespace PaletteMaster.Models.DTO.ImageProcessing;

public class ProcessFolderResponse
{
    public string OutputFolder { get; set; }
    
    public ProcessFolderResponse(string outputFolder)
    {
        OutputFolder = outputFolder;
    }
}