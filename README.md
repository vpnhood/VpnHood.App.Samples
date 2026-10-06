# VpnHood.Client.Samples
Samples for creating a VpnHood Client App

| Sample | What it shows |
| --- | --- |
| `CoreSample.WinConsole` | the engine alone on Windows: `VpnHoodClient` over a WinTun adapter, in a console. Run it as an administrator. |
| `CoreSample.AndroidForm` | the engine alone on Android: the VPN service through `VpnServiceManager`, in a plain activity. |
| `CoreSample.MauiForm` | the same in a MAUI app, on Android and Windows. |
| `AppLibSample.MauiForm` | the whole app (`VpnHood.AppUi.Hosting.Maui`) under MAUI pages of its own, which talk to it through its API. |

The SPA as a MAUI app's UI is a sample of `VpnHood.AppUi.Spa`, beside the SPA it shows.

The samples build against the VpnHood packages whose version `Directory.Build.props` pins. A local
version (`-localN`) comes from a `.packages` folder beside that file, which git ignores; a clean clone
restores only a published one.
