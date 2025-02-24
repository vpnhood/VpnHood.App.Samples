using Microsoft.Extensions.Logging;
using VpnHood.AppLib;
using VpnHood.AppLib.Maui.Common;
using VpnHood.AppLib.Resources;
using VpnHood.Core.Client.Abstractions;

namespace VpnHood.App.AppLibSample.MauiForm;

// ReSharper disable StringLiteralTypo
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
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
            Resource = resource, 
            AccessKeys = [ClientOptions.SampleAccessKey], // This is for test purpose only and can not be used in production
            EventWatcherInterval = TimeSpan.FromSeconds(1)
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