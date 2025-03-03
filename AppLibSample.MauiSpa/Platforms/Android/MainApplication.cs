using Android.App;
using Android.Runtime;

// ReSharper disable once CheckNamespace
namespace VpnHood.App.AppLibSample.MauiSpa;


[Application]
public class MainApplication(IntPtr handle, JniHandleOwnership ownership)
    : MauiApplication(handle, ownership)
{
    protected override MauiApp CreateMauiApp()
    {
        return MauiProgram.CreateMauiApp();
    }
}