using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Storage;
using Microsoft.Extensions.Logging;
using PaletteMaster.Presentation.Services;
using PaletteMaster.Repository;
using PaletteMaster.Services.ImageProcessing;
using PaletteMaster.Services.ImageSharp;
using PaletteMaster.Services.Palettes;

namespace PaletteMaster.Presentation;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        
        builder.Services.RegisterApplicationDbContext();
        
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts => { fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular"); })
            .UseMauiCommunityToolkit();

        builder.Services.AddMauiBlazorWebView();
        
#if MACCATALYST
        builder.Services.AddSingleton<IFilePickerService, MacOSFilePickerService>();
#else
        builder.Services.AddSingleton<IFilePickerService, StandardOSFilePickerService>();
#endif
        builder.Services.AddSingleton<IFolderPicker>(FolderPicker.Default);
        builder.Services.AddSingleton<IFileSaver>(FileSaver.Default);
        
        builder.Services.AddScoped<IPaletteRepository, PaletteRepository>();
        builder.Services.AddScoped<IPaletteService, PaletteService>();
        builder.Services.AddScoped<IImportPaletteService, ImportPaletteService>();
        builder.Services.AddScoped<IImageProcessingService, ImageSharpImageProcessingService>();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}