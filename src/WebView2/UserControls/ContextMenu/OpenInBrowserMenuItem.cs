using System.Windows.Controls;
using Util.Commands;
using Util.Utility;

namespace WebView2.UserControls.ContextMenu;

public class OpenInBrowserMenuItem : MenuItem
{
    public OpenInBrowserMenuItem(string url)
    {
        Header = "Open in browser";
        Command = new RelayCommand(_ => BrowserUtil.OpenInDefaultBrowser(url));
    }
}