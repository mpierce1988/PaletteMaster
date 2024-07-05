using PaletteMaster.Models.Domain;

namespace PaletteMaster.Services.ImageProcessing;

public static class ImageProcessingUtility
{
    public static Color MatchColor(Color target, List<Color> palette)
    {
        Color closest = palette[0];
        float minDistance = float.MaxValue;

        foreach (Color color in palette)
        {
            float distance = ColorDistance(target, color);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = color;
            }
        }

        return closest;
    }

    private static float ColorDistance(Color target, Color color)
    {
        return (float)Math.Sqrt(
            Math.Pow(target.Red - color.Red, 2) + 
            Math.Pow(target.Green - color.Green, 2) + 
            Math.Pow(target.Blue - color.Blue, 2)
            );
    }
}