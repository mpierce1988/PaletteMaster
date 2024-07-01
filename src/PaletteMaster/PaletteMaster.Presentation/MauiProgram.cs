using Microsoft.Extensions.Logging;
using PaletteMaster.Repository;
using PaletteMaster.Services.Palettes;

namespace PaletteMaster.Presentation;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts => { fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular"); });

        builder.Services.AddMauiBlazorWebView();
        builder.Services.RegisterApplicationDbContext();
        builder.Services.AddScoped<IPaletteRepository, PaletteRepository>();
        builder.Services.AddScoped<IPaletteService, PaletteService>();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}