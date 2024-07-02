using System.ComponentModel.DataAnnotations;

namespace PaletteMaster.Models.Domain;

public class Color : BaseEntity
{
    public int ColorId { get; set; }
    
    [Length(minimumLength: 7, maximumLength: 7, ErrorMessage="Hexadecimal must be at 7 characters long")]
    public string Hexadecimal { get; set; }

    public Color()
    {
    }
    
    public Color(string hexadecimal)
    {
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