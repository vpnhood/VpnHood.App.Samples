# CoreSample.Server

The VPN server engine in a console app: a VpnHood server that keeps its access keys in files, makes one on its first
run, prints it, and serves clients until you press Ctrl+C. By default it has no TUN adapter: it carries the clients'
traffic over connections of its own, so it needs no administrator. With `--tun`, it starts the platform's TUN adapter
with NAT, as the VpnHood server app does: the server then carries whole IP packets, and a client's TCP goes as packets
in the tunnel instead of being forwarded.

> [!TIP]
> Most people don't need to build a server: the VpnHood server app is ready to install, and it is the recommended way
> to run one. [Install the VpnHood server](https://www.vpnhood.com/vpnhood-server/docs/install/).

**Who it is for:** you run VpnHood servers from a .NET program of your own, with your own way to hand out keys or to
watch the server, or want to see the server engine with the least code around it.

## What you need

- Windows or Linux.
- The .NET SDK the project targets.
- A port clients can reach: open it in the firewall, and forward it to the machine where a router's NAT stands in
  front of it. On Linux, a port below 1024, such as 443, needs root.
- With `--tun`: root on Linux, with a TUN device (`/dev/net/tun`), or an administrator on Windows.

## Run it

In this folder, with the address clients reach the server at, and its port:

```powershell
dotnet run -- 203.0.113.10:9443
```

It prints the access key a client connects with, the same one on every run, as the key stays in the server's folder:

```text
Access key, for a client to connect with:
vh:eyJ2Ijo0LCJu...
The server is listening on port 9443. Press Ctrl+C to stop.
```

With the TUN adapter, from a root shell on Linux or a console run as an administrator on Windows:

```powershell
dotnet run -- 203.0.113.10:9443 --tun
```

It then says `The server is listening on port 9443, with a TUN adapter.`, and removes the adapter and its NAT when it
stops.

A client connects with that key in place of the test server's: `ClientOptions.AccessKey` in
[CoreSample.WinConsole](../CoreSample.WinConsole), for one.

## Read first

[Program.cs](Program.cs), all of it: the access manager (`FileAccessManager`) with the public endpoint and the port it
listens on, the key it keeps (`AccessTokenService`), the server (`VpnHoodServer`), and `StartTunAdapter`, the adapter
`--tun` gives it (`LinuxTunVpnAdapter` or `WinTunVpnAdapter`).

The server's folder is `VpnHood.App.CoreSample.Server` in your local app data: delete it to start again with a new
key.
