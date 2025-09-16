using VpnHood.AppLib;
using VpnHood.Core.Toolkit.Net;
using VpnHood.Core.Toolkit.Utils;

namespace VpnHood.App.AppLibSample.MauiForm;

// ReSharper disable once RedundantExtendsListEntry
public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        VpnHoodApp.Instance.ClearLastError();
        VpnHoodApp.Instance.ConnectionStateChanged += (_, _) => UpdateUi();
        UpdateUi();
    }

    private void OnConnectClicked(object sender, EventArgs e)
    {
        _ = ConnectTask();
    }

    private async Task ConnectTask()
    {
        try
        {
            if (VpnHoodApp.Instance.State.CanConnect)
                await VpnHoodApp.Instance.Connect();

            else if (VpnHoodApp.Instance.State.CanDisconnect)
                _ = VpnHoodApp.Instance.Disconnect();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private void UpdateUi()
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            StatusLabel.Text = VpnHoodApp.Instance.State.ConnectionState.ToString();
            CounterBtn.IsEnabled = VpnHoodApp.Instance.State.CanConnect || VpnHoodApp.Instance.State.CanDisconnect;
            CounterBtn.Text = VpnHoodApp.Instance.State.CanConnect ? "Connect" : "Disconnect";
            ErrorLabel.Text = VpnHoodApp.Instance.State.LastError != null ? $"Error: {VpnHoodApp.Instance.State.LastError?.Message}" : string.Empty;
        });

        _ = UpdateIp();
    }

    private AppConnectionState? _lastAppConnectionState;
    private async Task UpdateIp()
    {
        using var asyncLock = await AsyncLock.LockAsync("UpdateIp");
        var appConnectionState = VpnHoodApp.Instance.State.ConnectionState;
        if (_lastAppConnectionState != appConnectionState &&
            appConnectionState is AppConnectionState.Connected or AppConnectionState.None)
        {
            _lastAppConnectionState = VpnHoodApp.Instance.State.ConnectionState;
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