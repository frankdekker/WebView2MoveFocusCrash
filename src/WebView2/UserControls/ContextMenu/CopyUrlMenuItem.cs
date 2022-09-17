using System.Windows;
using System.Windows.Controls;
using Util.Commands;

namespace WebView2.UserControls.ContextMenu;

public class CopyUrlMenuItem : MenuItem
{
    public CopyUrlMenuItem(string url)
    {
        Header = "Copy url";
        Command = new RelayCommand(_ => Clipboard.SetText(url));
    }
}