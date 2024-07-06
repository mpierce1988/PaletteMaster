using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace PaletteMaster.Models.Domain;

public class Color : BaseEntity
{
    public int ColorId { get; set; }
    
    [Length(minimumLength: 7, maximumLength: 7, ErrorMessage="Hexadecimal must be at 7 characters long")]
    public string Hexadecimal { get; set; }

    [NotMapped] public int Red => Convert.ToInt32(Hexadecimal.Substring(1, 2), 16);

    [NotMapped] public int Green => Convert.ToInt32(Hexadecimal.Substring(3, 2), 16);

    [NotMapped] public int Blue => Convert.ToInt32(Hexadecimal.Substring(5, 2), 16);

    [NotMapped] public int Alpha => Hexadecimal.Length == 9 ? Convert.ToInt32(Hexadecimal.Substring(7, 2), 16) : 255;

    public Color()
    {
    }
    
    public Color(string hexadecimal)
    {
        if(!hexadecimal.StartsWith('#'))
        {
            hexadecimal = $"#{hexadecimal}";
        }

        if (hexadecimal.Length != 7 && hexadecimal.Length != 9)
            throw new ArgumentOutOfRangeException(nameof(hexadecimal), "Hexadecimal must be 7 or 9 characters long");
        
        Hexadecimal = hexadecimal;
    }

    public Color(Vector4 colorVector)
    {
        int red = (int) (colorVector.X * 255);
        int green = (int) (colorVector.Y * 255);
        int blue = (int) (colorVector.Z * 255);
        int alpha = (int) (colorVector.W * 255);
        
        Hexadecimal = $"#{red:X2}{green:X2}{blue:X2}{alpha:X2}";
    }

    public override bool Equals(object? obj)
    {
        return obj is Color color &&
               Hexadecimal == color.Hexadecimal;
    }

    public override int GetHashCode()
    {
        return Hexadecimal.GetHashCode();
    }

    public Vector4 ToVector4()
    {
        return new Vector4((int) Red / 255f, (int) Green / 255f, (int) Blue / 255f, (int) Alpha / 255f);
    }
}