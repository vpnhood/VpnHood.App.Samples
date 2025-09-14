using VpnHood.Core.Client.Device.UiContexts;

// ReSharper disable once CheckNamespace
namespace VpnHood.App.CoreSample.MauiForm.WinUI;

public class WinUiContext : IUiContext
{
    public Task<bool> IsActive() => Task.FromResult(true); //always true in sample
    public Task<bool> IsDestroyed() => Task.FromResult(false);
}