﻿
using VpnHood.AppLib;

namespace VpnHood.App.AppLibSample.MauiForm;
public partial class App
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = new Window(new AppShell())
        {
            Width = VpnHoodApp.Instance.Resources.WindowSize.Width,
            Height = VpnHoodApp.Instance.Resources.WindowSize.Height,
            Title = VpnHoodApp.Instance.Resources.Strings.AppName
        };
        return window;
    }

    protected override void CleanUp()
    {
        base.CleanUp();
        if (VpnHoodApp.IsInit) 
            _ = VpnHoodApp.Instance.DisposeAsync();
    }
}