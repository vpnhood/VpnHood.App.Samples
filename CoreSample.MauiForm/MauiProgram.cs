using Microsoft.Extensions.Logging;
using VpnHood.Core.Client.Device;
using VpnHood.Core.Client.VpnServices.Manager;

namespace VpnHood.App.CoreSample.MauiForm;

public static class MauiProgram
{
    public static VpnServiceManager VpnServiceManager { get; private set; } = null!;
    public static MauiApp CreateMauiApp(IDevice device)
    {
        VpnServiceManager = new VpnServiceManager(device, eventWatcherInterval: TimeSpan.FromSeconds(1));

        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Logging.AddDebug();
        return builder.Build();
    }
}