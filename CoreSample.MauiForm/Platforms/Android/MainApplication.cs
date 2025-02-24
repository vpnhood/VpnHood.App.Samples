using Android.App;
using Android.Runtime;
using VpnHood.Core.Client.Device.Droid;

namespace VpnHood.App.CoreSample.MauiForm;


[Application]
public class MainApplication(IntPtr handle, JniHandleOwnership ownership) 
    : MauiApplication(handle, ownership)
{

    protected override MauiApp CreateMauiApp()
    {
        var device = new AndroidDevice();
        var mauiApp = MauiProgram.CreateMauiApp(device);
        return mauiApp;
    }
}