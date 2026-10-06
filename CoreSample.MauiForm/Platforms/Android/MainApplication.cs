using Android.App;
using Android.Runtime;
using VpnHood.Core.Client.Devices.Android;

namespace VpnHood.App.CoreSample.MauiForm;


[Application]
public class MainApplication(IntPtr handle, JniHandleOwnership ownership) 
    : MauiApplication(handle, ownership)
{

    protected override MauiApp CreateMauiApp()
    {
        // Android runs the VPN service in a process of its own and builds this application there too;
        // the device, and the manager over it, belong to the app's own process only.
        var device = AndroidDevice.IsVpnServiceProcess ? null : AndroidDevice.Create();
        var mauiApp = MauiProgram.CreateMauiApp(device);
        return mauiApp;
    }
}