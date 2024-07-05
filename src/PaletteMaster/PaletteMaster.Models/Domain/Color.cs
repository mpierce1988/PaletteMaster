using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

    public override bool Equals(object? obj)
    {
        return obj is Color color &&
               Hexadecimal == color.Hexadecimal;
    }

    public override int GetHashCode()
    {
        return Hexadecimal.GetHashCode();
    }
}