using System.Windows.Controls;
using Util.Commands;
using WebView2.ViewModels;

namespace WebView2.UserControls.ContextMenu;

public class ReloadWithoutCacheMenuItem : MenuItem
{
    public ReloadWithoutCacheMenuItem(WebView2ViewModel viewModel)
    {
        Header = "Reload without cache";
        InputGestureText = "Ctrl+F5";
        Command = new RelayCommand(_ => viewModel.Reload.Invoke(), _ => viewModel.Reload != null);
    }
}