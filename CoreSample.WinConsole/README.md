# CoreSample.WinConsole

The VPN engine alone on Windows, in a console: `VpnHoodClient` over a WinTun adapter connects to VpnHood's sample
server, and you browse through it until you press Ctrl+C. No app around it: no UI, no settings, no reconnection.

**Who it is for:** you put the VPN into a Windows program or service of your own, or want to see the engine with the
least code around it: an adapter, a client, a connect. For an app with screens, see
[CoreSample.MauiForm](../CoreSample.MauiForm) or [AppLibSample.MauiForm](../AppLibSample.MauiForm).

## What you need

- Windows, and a console run as an administrator: the WinTun adapter adds a network interface.
- The .NET SDK the project targets.

## Run it

In this folder, from a console run as an administrator:

```powershell
dotnet run
```

It prints:

```text
Hello VpnClient!
Connecting...
Connected.

IP logging is enabled on these servers. Please follow United States law, especially if using torrent. Read privacy policy before use: https://github.com/vpnhood/VpnHood/blob/main/PRIVACY.md

VpnHood Client Is Running! Open your browser and browse the Internet!
This is a test server, and the session will be terminated in a few minutes.
Press Ctrl+C to stop.
```

## Read first

[Program.cs](Program.cs), all of it: the adapter (`WinTunVpnAdapter`), the client's options with the sample server's
access key (`ClientOptions.SampleAccessKey`), and `VpnHoodClient.Connect`.

The sample server is for tests: it ends each session after a few minutes. Your own app connects with an access key of
your own server.
