namespace PaletteMaster.Presentation;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new MainPage();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = base.CreateWindow(activationState);

        _ = window ?? throw new ArgumentNullException(nameof(window));
        
        window.Title = "Palette Master";

        return window;
    }
}