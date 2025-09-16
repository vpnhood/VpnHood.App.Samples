using VpnHood.Core.Client;
using VpnHood.Core.Client.Abstractions;
using VpnHood.Core.Tunneling.Sockets;
using VpnHood.Core.VpnAdapters.WinTun;

// ReSharper disable StringLiteralTypo
namespace VpnHood.App.CoreSample.WinConsole;

internal class Program
{
    private static async Task Main()
    {
        Console.WriteLine("Hello VpnClient!");

        // a clientId should be generated for each client
        var winVpnAdapter = new WinTunVpnAdapter(new WinVpnAdapterSettings
        {
            AdapterName = "TestAdapter",
            Blocking = true,
            AutoDisposePackets = true
        });

        var clientOptions = new ClientOptions
        {
            AppName = "TestApp",
            ClientId = Guid.Parse("7BD6C156-EEA3-43D5-90AF-B118FE47ED0A").ToString(),
            AccessKey = ClientOptions.SampleAccessKey // This is for test purpose only and can not be used in production
        };
        var vpnHoodClient = new VpnHoodClient(winVpnAdapter, new SocketFactory(), null, clientOptions);

        // connect to VpnHood server
        Console.WriteLine("Connecting...");
        await vpnHoodClient.Connect();
        Console.WriteLine("Connected.");

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\nIP logging is enabled on these servers. Please follow United States law, especially if using torrent. Read privacy policy before use: https://github.com/vpnhood/VpnHood/blob/main/PRIVACY.md\n");
        Console.ResetColor();

        Console.WriteLine("VpnHood Client Is Running! Open your browser and browse the Internet!");
        Console.WriteLine("This is a test server, and the session will be terminated in a few minutes.");
        Console.WriteLine("Press Ctrl+C to stop.");
        while (vpnHoodClient.State != ClientState.Disposed)
            await Task.Delay(1000);
    }
}