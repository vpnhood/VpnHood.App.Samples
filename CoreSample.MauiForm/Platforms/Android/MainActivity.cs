using Android.App;
using Android.OS;
using Android.Service.QuickSettings;
using VpnHood.Core.Client.Device.Droid;
using VpnHood.Core.Client.Device.UiContexts;

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
    }

    protected override void OnDestroy()
    {
        AppUiContext.Context = null;
        base.OnDestroy();
    }
}
