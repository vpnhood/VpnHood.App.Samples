using VpnHood.Core.Client.Abstractions;
using VpnHood.Core.Client.VpnServices.Abstractions;
using VpnHood.Core.Client.VpnServices.Manager;
using VpnHood.Core.Toolkit.Net;
using VpnHood.Core.Toolkit.Utils;

namespace VpnHood.App.CoreSample.MauiForm;

// ReSharper disable StringLiteralTypo
// ReSharper disable once RedundantExtendsListEntry
public partial class MainPage : ContentPage
{
    private VpnServiceManager VpnServiceManager => MauiProgram.VpnServiceManager;
    private ConnectionInfo ConnectionInfo => VpnServiceManager.ConnectionInfo;
    private bool CanDisconnect => ConnectionInfo.ClientState is 
        ClientState.Connecting or ClientState.Connected or ClientState.Initializing or 
        ClientState.WaitingForAd or ClientState.Waiting;

    public MainPage()
    {
        InitializeComponent();
        VpnServiceManager.StateChanged += (_, _) => UpdateUi();
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
            // disconnect if already connected
            if (CanDisconnect)
            {
                _ =  Disconnect();
                return;
            }

            // Clear error
            ErrorLabel.Text = string.Empty;

            // Connect
            await VpnServiceManager.Start(new ClientOptions
            {
                AppName = "TestApp",
                ClientId = Guid.Parse("7BD6C156-EEA3-43D5-90AF-B118FE47ED0B").ToString(),
                AccessKey = ClientOptions.SampleAccessKey // This is for test purpose only and can not be used in production
            }, CancellationToken.None);
        }
        catch (Exception ex)
        {
            ErrorLabel.Text = $"Error: {ex.Message}";
            Console.WriteLine(ex.Message);
        }
    }

    private void UpdateUi()
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            StatusLabel.Text = ConnectionInfo.ClientState.ToString();
            CounterBtn.Text = ConnectionInfo.ClientState is ClientState.None or ClientState.Disposed ? "Connect" : "Disconnect";
        });
        _ = UpdateIp();
    }

    private bool? _wasConnected;
    private async Task UpdateIp()
    {
        using var asyncLock = await AsyncLock.LockAsync("UpdateIp");
        var isConnected = ConnectionInfo.ClientState is ClientState.Connected;
        if (_wasConnected == null || _wasConnected.Value != isConnected)
        {
            _wasConnected = isConnected;
            using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            var ipAddresses = await IPAddressUtil.GetPublicIpAddresses(cancellationTokenSource.Token);
            MainThread.BeginInvokeOnMainThread(() =>
            {
                IpLabel.Text = string.Join(" , ", ipAddresses.Select(x => x.ToString()));
                IpLabel.TextColor = isConnected ? Colors.Green : Colors.Red;
            });
        }
    }

    private async Task Disconnect()
    {
        await VpnServiceManager.Stop();
        UpdateUi();
    }
}