using VpnHood.Core.Client.Device.UiContexts;

// ReSharper disable once CheckNamespace
namespace VpnHood.App.CoreSample.MauiForm.WinUI;

public class WinUiContext : IUiContext
{
    public bool IsActive => true; //always true in sample
}