using Microsoft.Extensions.Logging;
using VpnHood.AppLib;
using VpnHood.AppLib.Resources;
using VpnHood.AppLib.WebServer;
using VpnHood.AppLib.Maui.Common;
using VpnHood.Core.Client.Abstractions;

namespace VpnHood.App.AppLibSample.MauiSpa;

// ReSharper disable StringLiteralTypo
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp
            .CreateBuilder()
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        var resource = DefaultAppResource.Resources;
        resource.Strings.AppName = "VpnHood Client Sample";
        VpnHoodMauiApp.Init(new AppOptions("com.vpnhood.client.sample", "VpnHoodSample", IsDebugMode)
        {
            StorageFolderPath = AppOptions.BuildStorageFolderPath("VpnHoodSample"),
            Resource = resource,
            AccessKeys = [ClientOptions.SampleAccessKey] // This is for test purpose only and can not be used in production
        });

        // init web server with spa zip data
        ArgumentNullException.ThrowIfNull(VpnHoodApp.Instance.Resource.SpaZipData);
        using var spaZipStream = new MemoryStream(VpnHoodApp.Instance.Resource.SpaZipData);
        VpnHoodAppWebServer.Init(new WebServerOptions
        {
            SpaZipStream = spaZipStream
        });

        if (IsDebugMode)
            builder.Logging.AddDebug();

        return builder.Build();
    }

#if DEBUG
    public static bool IsDebugMode => true;
#else
    public static bool IsDebugMode => false;
#endif
}