namespace VpnHood.App.AppLibSample.MauiForm;

public partial class App
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        // a phone's shape where the app has a window of its own size
        var window = new Window(new AppShell())
        {
            Width = 400,
            Height = 700,
            Title = "VpnHood! AppLib Sample"
        };
        return window;
    }

}
