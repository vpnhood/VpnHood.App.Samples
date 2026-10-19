using Android.App;
using Android.OS;
using Android.Service.QuickSettings;
using Android.Views;
using VpnHood.AppLib.App.Android.Activities;
using VpnHood.AppLib.App.Android.Constants;
using VpnHood.AppUi.Hosting.Maui;
using VpnHood.Net.Toolkit.Graphics;

// ReSharper disable once CheckNamespace
namespace VpnHood.App.AppLibSample.MauiForm;

[Activity(
    Theme = "@style/Maui.SplashTheme",
    MainLauncher = true,
    LaunchMode = AndroidMainActivityConstants.LaunchMode,
    Exported = AndroidMainActivityConstants.Exported,
    WindowSoftInputMode = AndroidMainActivityConstants.WindowSoftInputMode,
    ScreenOrientation = AndroidMainActivityConstants.ScreenOrientation,
    ConfigurationChanges = AndroidMainActivityConstants.ConfigChanges)]

[IntentFilter([TileService.ActionQsTilePreferences])]
public class MainActivity : VpnHoodMauiMainActivity
{
    private const int DarkBarIcons =
        (int)(WindowInsetsControllerAppearance.LightStatusBars | WindowInsetsControllerAppearance.LightNavigationBars);

    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        // dark bar icons over this sample's light page: the app's window, behind the bars from
        // Android 11, gives them light ones, for VpnHood's own dark UIs
        if (OperatingSystem.IsAndroidVersionAtLeast(30))
            Window?.InsetsController?.SetSystemBarsAppearance(DarkBarIcons, DarkBarIcons);
    }

    protected override AndroidAppMainActivityHandler CreateMainActivityHandler()
    {
        // no theme colour for the system bars: they keep the system's around this sample's own pages
        return new AndroidAppMainActivityHandler(this, new AndroidMainActivityOptions(), Task.FromResult<VhColor?>(null));
    }
}
