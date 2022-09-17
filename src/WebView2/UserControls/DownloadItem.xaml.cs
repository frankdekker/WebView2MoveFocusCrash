using System.Windows;
using System.Windows.Input;

namespace WebView2.UserControls;

public partial class DownloadItem
{
    public DownloadItem()
    {
        InitializeComponent();
    }

    private void OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        e.Handled = true;
        if (sender is not FrameworkElement { ContextMenu: { } } element) {
            return;
        }

        element.ContextMenu.PlacementTarget = element;
        element.ContextMenu.IsOpen = element.ContextMenu.IsOpen == false;
    }
}