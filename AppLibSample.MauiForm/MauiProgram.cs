using Microsoft.Extensions.Logging;
using VpnHood.AppLib;
using VpnHood.AppLib.Abstractions;
using VpnHood.AppLib.Assets.ClassicSpa;
using VpnHood.AppLib.Assets.Ip2LocationLite;
using VpnHood.AppLib.Maui.Common;
using VpnHood.Core.Client.Abstractions;
using VpnHood.Core.Toolkit.Graphics;

namespace VpnHood.App.AppLibSample.MauiForm;

// ReSharper disable UseObjectOrCollectionInitializer
// ReSharper disable StringLiteralTypo
public static class MauiProgram
{
    private static AppOptions CreateAppOptions()
    {
        var resource = new AppResources();
        resource.IpLocationZipData = Ip2LocationLiteDb.ZipData;
        resource.SpaZipData = ClassicSpaResources.SpaZipData;
        resource.Colors.NavigationBarColor = VhColor.Parse(ClassicSpaResources.NavigationBarColor);
        resource.Colors.WindowBackgroundColor = VhColor.Parse(ClassicSpaResources.WindowBackgroundColor);
        resource.Colors.ProgressBarColor = VhColor.Parse(ClassicSpaResources.ProgressBarColor); 
        resource.Strings.AppName = "VpnHood Client Sample";
        resource.Strings.AppName = "VpnHood Client Sample";

        return new AppOptions("com.vpnhood.client.sample", "VpnHoodSample", IsDebugMode)
        {
            Resources = resource,
            AccessKeys = [ClientOptions.SampleAccessKey], // This is for test purpose only and can not be used in production
            EventWatcherInterval = TimeSpan.FromSeconds(1)
        };
    }

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

        // init VpnHood maui app
        VpnHoodAppMaui.Init(CreateAppOptions);

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