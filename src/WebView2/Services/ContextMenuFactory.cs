using Microsoft.Web.WebView2.Core;
using System.Windows.Controls;

namespace WebView2.Services;

public static class ContextMenuFactory
{
    public static MenuItem From(CoreWebView2ContextMenuRequestedEventArgs args, CoreWebView2ContextMenuItem item)
    {
        MenuItem newItem = new() {
            // Replace with '_' so it is underlined in the label
            // The accessibility key is the key after the & in the label
            Header = item.Label.Replace('&', '_'), Name = item.Name, InputGestureText = item.ShortcutKeyDescription, IsEnabled = item.IsEnabled
        };
        newItem.Click += (_, _) => args.SelectedCommandId = item.CommandId;
        return newItem;
    }
}