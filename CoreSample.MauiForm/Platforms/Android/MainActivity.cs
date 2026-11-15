using Android.App;
using Android.OS;
using Android.Service.QuickSettings;
using Android.Views;
using VpnHood.Core.Client.Devices.Android;
using VpnHood.Core.Client.Devices.Abstractions.UiContexts;

namespace VpnHood.App.CoreSample.MauiForm;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true)]

[IntentFilter([TileService.ActionQsTilePreferences])]
public class MainActivity : MauiActivityEvent
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        // required for granting access in VpnServiceManager.Start
        AppUiContext.Context = new AndroidUiContext(this);

        // white system bars with dark icons, over the white page, as AppLibSample.MauiForm has; from Android 15
        // they are clear over the page already
        if (OperatingSystem.IsAndroidVersionAtLeast(30))
        {
            const int darkBarIcons =
                (int)(WindowInsetsControllerAppearance.LightStatusBars | WindowInsetsControllerAppearance.LightNavigationBars);
            Window?.InsetsController?.SetSystemBarsAppearance(darkBarIcons, darkBarIcons);
            if (!OperatingSystem.IsAndroidVersionAtLeast(35))
            {
                Window?.SetStatusBarColor(Android.Graphics.Color.White);
                Window?.SetNavigationBarColor(Android.Graphics.Color.White);
            }
        }
    }

    protected override void OnDestroy()
    {
        AppUiContext.Context = null;
        base.OnDestroy();
    }
}
