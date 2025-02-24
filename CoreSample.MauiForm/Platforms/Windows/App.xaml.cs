using VpnHood.Core.Client.Device.WinDivert;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

// ReSharper disable once CheckNamespace
namespace VpnHood.App.CoreSample.MauiForm.WinUI;
/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
// ReSharper disable once RedundantExtendsListEntry
public partial class App : MauiWinUIApplication
{
    /// <summary>
    /// Initializes the singleton application object.  This is the first line of authored code
    /// executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    public App()
    {
        InitializeComponent();
    }

    protected override MauiApp CreateMauiApp()
    {
        var storageFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "VpnHood.App.CoreSample.MauiForm");
        var device =  new WinDevice(storageFolder, isDebugMode: true);
        return MauiProgram.CreateMauiApp(device);
    }

}