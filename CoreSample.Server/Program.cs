using System.Net;
using VpnHood.Core.Common.Configuration;
using VpnHood.Core.Server;
using VpnHood.Core.Server.Access.Managers.FileAccessManagers;
using VpnHood.Net.VpnAdapters.Abstractions;
using VpnHood.Net.VpnAdapters.LinuxTun;
using VpnHood.Net.VpnAdapters.WinTun;

namespace VpnHood.App.CoreSample.Server;

internal class Program
{
    private static async Task Main(string[] args)
    {
        // where clients reach the server: the machine's public address, or the one a firewall or NAT forwards
        // to it, and the port the server listens on; --tun gives the server a TUN adapter
        var useTun = args.Contains("--tun");
        var endPointArgs = args.Where(x => x != "--tun").ToArray();
        if (endPointArgs.Length != 1 || !IPEndPoint.TryParse(endPointArgs[0], out var publicEndPoint) ||
            publicEndPoint.Port == 0) {
            Console.WriteLine("Usage: dotnet run -- <public address>:<port> [--tun]");
            Console.WriteLine("such as: dotnet run -- 203.0.113.10:9443");
            return;
        }

        if (useTun && !Environment.IsPrivilegedProcess) {
            Console.WriteLine("--tun needs root on Linux, or an administrator on Windows.");
            return;
        }

        using var stopSource = new CancellationTokenSource();
        Console.CancelKeyPress += (_, e) => {
            e.Cancel = true;
            stopSource.Cancel();
        };

        // the server's own folder, its access keys in it; they stay between runs
        var dataFolder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "VpnHood.App.CoreSample.Server");

        // who may connect: access keys kept in files
        var accessManager = await FileAccessManager.Create(
            Path.Combine(dataFolder, "access"),
            new FileAccessManagerOptions {
                PublicEndPoints = [publicEndPoint],
                TcpEndPoints = [
                    new IPEndPoint(IPAddress.Any, publicEndPoint.Port),
                    new IPEndPoint(IPAddress.IPv6Any, publicEndPoint.Port)
                ]
            }, stopSource.Token);

        // one access key, made on the first run and kept: give it to the person who connects with it
        var tokenIdFile = Path.Combine(dataFolder, "sample-key-id.txt");
        var accessToken = File.Exists(tokenIdFile)
            ? (await accessManager.AccessTokenService.Get(File.ReadAllText(tokenIdFile), stopSource.Token)).AccessToken
            : await accessManager.AccessTokenService.Create(stopSource.Token, maxClientCount: 1, tokenName: "Sample key");
        File.WriteAllText(tokenIdFile, accessToken.TokenId);
        Console.WriteLine("Access key, for a client to connect with:");
        Console.WriteLine(accessManager.GetToken(accessToken).ToAccessKey());

        // with --tun, the platform's TUN adapter; disposed after the server, which uses it until then
        using var vpnAdapter = useTun ? await StartTunAdapter(stopSource.Token) : null;

        // the server, which disposes the access manager with itself
        await using var server = new VpnHoodServer(accessManager, new ServerOptions {
            StoragePath = dataFolder,
            VpnAdapter = vpnAdapter
        });
        await server.Start(stopSource.Token);
        Console.WriteLine($"The server is listening on port {publicEndPoint.Port}" +
                          (vpnAdapter != null ? ", with a TUN adapter." : ".") + " Press Ctrl+C to stop.");

        try {
            await Task.Delay(Timeout.Infinite, stopSource.Token);
        }
        catch (OperationCanceledException) {
            Console.WriteLine("Stopping...");
        }
    }

    // The platform's TUN adapter, started with NAT, as the VpnHood server app starts it: the server then carries whole
    // IP packets through it, and a client's TCP goes as packets in the tunnel instead of being forwarded. Without one,
    // the server forwards TCP, UDP and ping over connections of its own.
    private static async Task<IVpnAdapter> StartTunAdapter(CancellationToken cancellationToken)
    {
        IVpnAdapter adapter = OperatingSystem.IsWindows()
            ? new WinTunVpnAdapter(new WinVpnAdapterSettings {
                AdapterName = "VpnHoodSample",
                Blocking = false,
                AutoDisposePackets = true,
                AutoMetric = false // the server routes nothing of its own through the adapter
            })
            : new LinuxTunVpnAdapter(new LinuxVpnAdapterSettings {
                AdapterName = "VpnHoodSample",
                Blocking = false,
                AutoDisposePackets = true
            });

        // the server's own virtual networks, which the server hands its clients' addresses from
        var serverDefaults = new ServerOptions();
        await adapter.Start(new VpnAdapterOptions {
            SessionName = "VpnHoodSampleServer",
            Mtu = TransportDefaults.MtuServer,
            UseNat = true,
            VirtualIpNetworkV4 = serverDefaults.VirtualIpNetworkV4,
            VirtualIpNetworkV6 = serverDefaults.VirtualIpNetworkV6
        }, cancellationToken);
        return adapter;
    }
}
