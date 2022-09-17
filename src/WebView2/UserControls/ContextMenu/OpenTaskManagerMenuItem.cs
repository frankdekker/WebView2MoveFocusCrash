using Microsoft.Web.WebView2.Core;
using System;
using System.Windows.Controls;
using Util.Commands;

namespace WebView2.UserControls.ContextMenu;

public class OpenTaskManagerMenuItem : MenuItem
{
    public OpenTaskManagerMenuItem(CoreWebView2 coreWebView2)
    {
        Header = "Open taskmanager";
        Command = new RelayCommand(_ =>
        {
            try {
                coreWebView2.OpenTaskManagerWindow();
            } catch (Exception) {
                //ignored
            }
        });
    }
}