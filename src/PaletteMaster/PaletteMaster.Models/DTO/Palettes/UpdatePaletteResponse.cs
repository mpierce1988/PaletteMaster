using PaletteMaster.Models.Domain;

namespace PaletteMaster.Models.DTO.Palettes;

public class UpdatePaletteResponse : CreatePaletteResponse
{
    public DateTime? ModifiedDate { get; set; }

    public UpdatePaletteResponse()
    {
    }
    
    public UpdatePaletteResponse(Palette palette) : base(palette)
    {
        ModifiedDate = palette.ModifiedDate;
    }
}