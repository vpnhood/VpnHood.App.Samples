using Android.Graphics;
using VpnHood.Core.Client.Abstractions;
using VpnHood.Core.Client.Device.Droid;
using VpnHood.Core.Client.Device.Droid.ActivityEvents;
using VpnHood.Core.Client.Device.UiContexts;
using VpnHood.Core.Client.VpnServices.Abstractions;
using VpnHood.Core.Client.VpnServices.Manager;
using VpnHood.Core.Toolkit.Net;
using VpnHood.Core.Toolkit.Utils;

// ReSharper disable StringLiteralTypo
namespace VpnHood.App.CoreSample.AndroidForm;

[Activity(Label = "@string/app_name", MainLauncher = true)]
// ReSharper disable once UnusedMember.Global
public class MainActivity : ActivityEvent
{
    private Button _connectButton = null!;
    private TextView _statusTextView = null!;
    private TextView _ipTextView = null!;

    private VpnServiceManager VpnServiceManager { get; } = new(new AndroidDevice(), TimeSpan.FromSeconds(1));
    private ConnectionInfo ConnectionInfo => VpnServiceManager.ConnectionInfo;
    private bool CanDisconnect => ConnectionInfo.ClientState is
        ClientState.Connecting or ClientState.Connected or ClientState.Initializing or
        ClientState.WaitingForAd or ClientState.Waiting;


    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        VpnServiceManager.StateChanged += (_, _) => UpdateUi();

        // Set our simple view
        var mainView = new LinearLayout(this);
        mainView.SetPadding(20, 10, 20, 10);
        mainView.Orientation = Orientation.Vertical;

        // button and status
        var linearLayout = new LinearLayout(this);
        _connectButton = new Button(this);
        _connectButton.Click += ConnectButton_Click;
        linearLayout.AddView(_connectButton);
        mainView.AddView(linearLayout);

        _statusTextView = new TextView(this);
        _statusTextView.SetTextColor(Color.DarkGreen);
        linearLayout.AddView(_statusTextView);

        _ipTextView = new TextView(this);
        _ipTextView.SetTextColor(Color.DarkGreen);
        _ipTextView.SetPadding(_ipTextView.PaddingLeft, _ipTextView.PaddingTop + 30, _ipTextView.PaddingRight, _ipTextView.PaddingBottom);
        mainView.AddView(_ipTextView);

        // notes
        var note = new TextView(this);
        note.SetPadding(note.PaddingLeft, note.PaddingTop + 30, note.PaddingRight, note.PaddingBottom);
        note.SetTextColor(Color.Blue);
        note.Text = "This sample demonstrates how to connect to a VpnHoodServer using the VpnHoodClient (core). However, developing a fully functional VPN application involves much more, including handling UI commands, accounting, billing, advertising, notifications, managing keys, handling reconnections, handling exceptions, acquiring permissions, leveraging OS features such as Tile, Always ON, among other considerations. Consider using VpnHood.AppLib, which provides a comprehensive set of extended functionalities.";
        mainView.AddView(note);

        note = new TextView(this);
        note.SetPadding(note.PaddingLeft, note.PaddingTop + 30, note.PaddingRight, note.PaddingBottom);
        note.SetTextColor(Color.IndianRed);
        note.Text = "* This is a test server, and the session will be terminated in a few minutes.";
        mainView.AddView(note);

        note = new TextView(this);
        note.SetPadding(note.PaddingLeft, note.PaddingTop + 30, note.PaddingRight, note.PaddingBottom);
        note.SetTextColor(Color.Black);
        note.Text = "* Please contact us if this sample is outdated or not working. Report issues at: https://github.com/vpnhood/VpnHood/issues";
        mainView.AddView(note);

        SetContentView(mainView);
        UpdateUi();
    }

    private void ConnectButton_Click(object? sender, EventArgs e)
    {
        Task.Run(ConnectTask);
    }

    private async Task ConnectTask()
    {
        try
        {
            // disconnect if already connected
            if (CanDisconnect)
            {
                _= Disconnect();
                return;
            }

            // start vpn 
            await VpnServiceManager.Start(new ClientOptions
            {
                AppName = "TestApp",
                ClientId = Guid.Parse("7BD6C156-EEA3-43D5-90AF-B118FE47ED0B").ToString(),
                AccessKey = ClientOptions.SampleAccessKey // This is for test purpose only and can not be used in production
            }, CancellationToken.None);

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private async Task Disconnect()
    {
        await VpnServiceManager.TryStop();
        UpdateUi();
    }

    private void UpdateUi()
    {
        RunOnUiThread(() =>
        {
            _statusTextView.Text = ConnectionInfo.ClientState.ToString();
            _statusTextView.SetTextColor(ConnectionInfo.ClientState is ClientState.Connected ? Color.DarkGreen : Color.DarkRed);
            _connectButton.Text = ConnectionInfo.ClientState is ClientState.None or ClientState.Disposed ? "Connect" : "Disconnect";
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
            RunOnUiThread(() =>
            {
                _ipTextView.Text = " ( " + string.Join(" , ", ipAddresses.Select(x => x.ToString())) + " )";
                _ipTextView.SetTextColor(isConnected  ? Color.DarkGreen : Color.DarkRed);
            });
        }
    }

    protected override void OnResume()
    {
        AppUiContext.Context = new AndroidUiContext(this);
        base.OnResume();
    }

    protected override void OnPause()
    {
        AppUiContext.Context = null;
        base.OnPause();
    }
}