using Microsoft.EntityFrameworkCore;
using PaletteMaster.Repository;

namespace PaletteMaster.Presentation;

public partial class App : Application
{
    private readonly ApplicationDbContext _context;
    public App(ApplicationDbContext context)
    {
        InitializeComponent();

        MainPage = new MainPage();
        _context = context;
    }

    protected override void OnStart()
    {
        _context.Database.Migrate();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = base.CreateWindow(activationState);

        _ = window ?? throw new ArgumentNullException(nameof(window));
        
        window.Title = "Palette Master";

        return window;
    }
}