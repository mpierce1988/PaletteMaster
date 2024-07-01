using PaletteMaster.Models.Domain;
using PaletteMaster.Models.DTO.Palettes;
using PaletteMaster.Services.Palettes;

namespace PaletteMaster.Repository;

public class MockPaletteRepository : IPaletteRepository
{
    private List<Palette> _examplePalettes = new()
    {
        new Palette()
        {
            Name = "Example Palette 1",
            CreatedDate = DateTime.Now.AddDays(-7),
            ModifiedDate = DateTime.Now.AddDays(-1),
            Colors = new List<Color>()
            {
                new Color("#FF0000"),

                new Color("#00FF00"),
                new Color("#0000FF")
            }
        },
        new Palette()
        {
            Name = "Example Palette 2",
            CreatedDate = DateTime.Now.AddDays(-14),
            ModifiedDate = DateTime.Now.AddDays(-2),
            Colors = new List<Color>()
            {
                new Color("#FF00FF"),
                new Color("#FFFF00"),
                new Color("#00FFFF")
            }
        },
    };
    
    public async Task<List<Palette>> GetPalettesAsync(GetPalettesRequest request)
    {
        await Task.Delay(TimeSpan.FromSeconds(1));

        return _examplePalettes;
    }

    public async Task<int> GetPalettesCountAsync(GetPalettesRequest request)
    {
        await Task.Delay(TimeSpan.FromSeconds(1));

        return _examplePalettes.Count;
    }

    public async Task<Palette?> GetPaletteAsync(int requestPalletId)
    {
        await Task.Delay(TimeSpan.FromSeconds(1));
        
        return _examplePalettes[0];
    }

    public async Task<Palette?> GetPaletteWithUseTrackingAsync(int requestPalleteId)
    {
        await Task.Delay(TimeSpan.FromSeconds(1));
        
        var resultWithTrackingData = _examplePalettes[0];
        
        resultWithTrackingData.PaletteUseTrackings = new List<PaletteUseTracking>()
        {
            new PaletteUseTracking()
            {
                PaletteUseTrackingId = 1,
                PaletteId = 1,
                CreatedDate = DateTime.Now.AddDays(-7)
            },
            new PaletteUseTracking()
            {
                PaletteUseTrackingId = 2,
                PaletteId = 1,
                CreatedDate = DateTime.Now.AddDays(-5)
            },
            new PaletteUseTracking()
            {
                PaletteUseTrackingId = 3,
                PaletteId = 1,
                CreatedDate = DateTime.Now.AddDays(-3)
            },
        };

        return resultWithTrackingData;
    }

    public async Task<Palette> CreatePaletteAsync(Palette palette)
    {
        await Task.Delay(TimeSpan.FromSeconds(1));
        
        // get next ID
        var nextId = _examplePalettes.Max(p => p.PaletteId) + 1;
        palette.PaletteId = nextId;
        
        _examplePalettes.Add(palette);
        
        return palette;
    }
}