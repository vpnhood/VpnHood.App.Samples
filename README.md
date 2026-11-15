# VpnHood.App.Samples

Small apps that each show one part of VpnHood at work, built on its NuGet packages, and connect to VpnHood's sample
server. Each folder's README says how to run it and which file to read first.

Which one to read depends on how much of the app you want to write yourself:

- **The VPN engine alone** (`CoreSample.*`): you write the app around it, its screens and everything a VPN app does
  beyond connecting.
- **The app's logic** (`AppLibSample.MauiForm`): the app is ready, with reconnection, access keys, split tunneling and
  notifications, and only its screens are yours.
- **The whole app**, VpnHood's own screens too: start from
  [VpnHood.App.Template](https://github.com/vpnhood/VpnHood.App.Template) instead.

| Sample | What it shows | Who it is for |
| --- | --- | --- |
| [CoreSample.WinConsole](CoreSample.WinConsole) | The engine alone on Windows, in a console: `VpnHoodClient` over a WinTun adapter connects, with no UI. Run it as an administrator. | You put the VPN into a Windows program or service of your own, or want to see the engine with the least code around it: an adapter, a client, a connect. |
| [CoreSample.AndroidForm](CoreSample.AndroidForm) | The engine alone on Android, in a plain activity: the VPN service started through `VpnServiceManager`, with the connection's state and the public IP on one page. | You write an Android app in .NET for Android, without MAUI, with screens of your own, and handle access keys, reconnection and the rest yourself. |
| [CoreSample.MauiForm](CoreSample.MauiForm) | The same engine in a MAUI app, on Android and Windows, each platform giving it its device. | You write a MAUI app and want the VPN in it on both platforms, and handle the rest yourself. |
| [AppLibSample.MauiForm](AppLibSample.MauiForm) | The whole app's logic (`VpnHood.AppUi.Hosting.Maui`) under a MAUI page of its own, which talks to it through the app's API. | You want a complete VPN app under screens of your own, without writing what a VPN app does beyond connecting. |

The samples build against the VpnHood packages whose version `Directory.Build.props` pins. A local
version (`-localN`) comes from a `.packages` folder beside that file, which git ignores; a clean clone
restores only a published one.
