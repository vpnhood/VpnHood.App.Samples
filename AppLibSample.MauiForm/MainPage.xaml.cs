using VpnHood.AppLib.Api;
using VpnHood.AppLib.Api.App;
using VpnHood.AppUi.Hosting.Maui;
using VpnHood.Net.Toolkit.Net;
using VpnHood.Net.Toolkit.Utils;

namespace VpnHood.App.AppLibSample.MauiForm;

// ReSharper disable once RedundantExtendsListEntry
public partial class MainPage : ContentPage
{
    // the app's API, the same calls every VpnHood UI makes
    private static VpnHoodApi Api => VpnHoodAppMaui.Instance.Api;

    public MainPage()
    {
        InitializeComponent();
        _ = Api.App.ClearLastError(CancellationToken.None);

        // the API answers calls and raises no events, so the page reads the state once a second, and
        // at once when it shows
        Loaded += (_, _) => _ = UpdateUi();
        Dispatcher.StartTimer(TimeSpan.FromSeconds(1), () =>
        {
            _ = UpdateUi();
            return true;
        });
    }

    private void OnConnectClicked(object sender, EventArgs e)
    {
        _ = ConnectTask();
    }

    private async Task ConnectTask()
    {
        try
        {
            var state = await Api.App.GetState(CancellationToken.None);
            if (state.CanConnect)
                await Api.App.Connect(vpnProfileId: null, serverLocation: null, ConnectPlanId.Normal, CancellationToken.None);

            else if (state.CanDisconnect)
                await Api.App.Disconnect(CancellationToken.None);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private async Task UpdateUi()
    {
        var state = await Api.App.GetState(CancellationToken.None);

        // the app's window reaches behind the system bars, and the state gives their heights in
        // pixels: the page keeps clear of them, as every VpnHood UI does
        var density = DeviceDisplay.MainDisplayInfo.Density;
        Padding = new Thickness(0, state.SystemBarsInfo.TopHeight / density, 0, state.SystemBarsInfo.BottomHeight / density);

        StatusLabel.Text = state.ConnectionState.ToString();
        CounterBtn.IsEnabled = state.CanConnect || state.CanDisconnect;
        CounterBtn.Text = state.CanConnect ? "Connect" : "Disconnect";
        ErrorLabel.Text = state.LastError != null ? $"Error: {state.LastError.Message}" : string.Empty;

        _ = UpdateIp(state.ConnectionState);
    }

    private AppConnectionState? _lastAppConnectionState;
    private async Task UpdateIp(AppConnectionState appConnectionState)
    {
        using var asyncLock = await AsyncLock.LockAsync("UpdateIp");
        if (_lastAppConnectionState != appConnectionState &&
            appConnectionState is AppConnectionState.Connected or AppConnectionState.None)
        {
            _lastAppConnectionState = appConnectionState;
            await Task.Delay(1000);

            using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            var ipAddresses = await IPAddressUtil.GetPublicIpAddresses(cancellationTokenSource.Token);
            MainThread.BeginInvokeOnMainThread(() =>
            {
                IpLabel.Text = string.Join(" , ", ipAddresses.Select(x => x.ToString()));
                IpLabel.TextColor = appConnectionState is AppConnectionState.Connected ? Colors.Green : Colors.Red;
            });
        }
    }
}
