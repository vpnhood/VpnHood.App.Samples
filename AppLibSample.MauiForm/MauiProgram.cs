using Microsoft.Extensions.Logging;
using VpnHood.AppLib.App;
using VpnHood.AppUi.Hosting.Maui;
using VpnHood.Core.Client.Abstractions;

namespace VpnHood.App.AppLibSample.MauiForm;

// ReSharper disable StringLiteralTypo
public static class MauiProgram
{
    // Who the app is and its options: what every VpnHood app hands its platform. The app runs in this
    // process, and the pages talk to it through VpnHoodAppMaui.Instance.Api.
    private static AppInitParams CreateInitParams()
    {
        return new AppInitParams
        {
            AppId = "com.vpnhood.applibsample.mauiform",
            AppName = "VpnHood! AppLib Sample",
            AppVersion = typeof(MauiProgram).Assembly.GetName().Version ??
                         throw new InvalidOperationException("The sample's assembly has no version."),
            StorageFolderName = "VpnHoodAppLibSample",
            AppOptionsFactory = context => new AppOptions(context, IsDebugMode)
            {
                PackageTitle = "VpnHoodAppLibSample",
                CompanyName = "VpnHood",
                // the logo and the consent summary in a UI store; this sample's own pages read neither
                LogoAssetPath = "images/logo.png",
                PrivacyConsentAssetName = "privacy-consent",
                AccessKeys = [ClientOptions.SampleAccessKey], // This is for test purpose only and can not be used in production
                EventWatcherInterval = TimeSpan.FromSeconds(1)
            }
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
        VpnHoodAppMaui.Init(CreateInitParams());

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
