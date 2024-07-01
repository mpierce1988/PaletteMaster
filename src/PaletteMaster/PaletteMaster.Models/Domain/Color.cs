using System.ComponentModel.DataAnnotations;

namespace PaletteMaster.Models.Domain;

public class Color : BaseEntity
{
    public int ColorId { get; set; }
    
    [Range(minimum:0, maximum:255, ErrorMessage = "Red must be between 0 and 255")]
    public int Red { get; set; }
    
    [Range(minimum:0, maximum:255, ErrorMessage = "Red must be between 0 and 255")]
    public int Green { get; set; }
    
    [Range(minimum:0, maximum:255, ErrorMessage = "Red must be between 0 and 255")]
    public int Blue { get; set; }

    public Color(int red, int green, int blue)
    {
        Red = red;
        Green = green;
        Blue = blue;
    }

    public Color(string hexadecimal)
    {
        if (hexadecimal.StartsWith('#'))
        {
            hexadecimal = hexadecimal[1..]; // hexadecimal.Substring(1);
        }

        Red = Convert.ToInt32(hexadecimal.Substring(0, 2), 16);
        Green = Convert.ToInt32(hexadecimal.Substring(2, 2), 16);
        Blue = Convert.ToInt32(hexadecimal.Substring(4, 2), 16);
    }
    
    public string ToHex()
    {
        return $"#{Red:X2}{Green:X2}{Blue:X2}";
    }
}