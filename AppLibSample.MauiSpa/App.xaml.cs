using VpnHood.AppLib;
using VpnHood.AppLib.Maui.Common;
using VpnHood.AppLib.WebServer;

namespace VpnHood.App.AppLibSample.MauiSpa;

public partial class App
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = new Window(new MainPage())
        {
            Width = VpnHoodApp.Instance.Resources.WindowSize.Width,
            Height = VpnHoodApp.Instance.Resources.WindowSize.Height,
            Title = VpnHoodApp.Instance.Resources.Strings.AppName
        };
        return window;
    }

    protected override void CleanUp()
    {
        base.CleanUp();
        if (VpnHoodAppWebServer.IsInit) VpnHoodAppWebServer.Instance.Dispose();
        if (VpnHoodMauiApp.IsInit) VpnHoodMauiApp.Instance.Dispose();
    }
}