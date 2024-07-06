using PaletteMaster.Models.Domain;

namespace PaletteMaster.Services.ImageProcessing;

public interface IImageProcessingUtility
{
    public Color MatchColor(Color originalColor, List<Color> allowedColors);
}