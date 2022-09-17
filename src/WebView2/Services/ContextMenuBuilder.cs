#nullable enable
using Microsoft.Web.WebView2.Core;
using System;
using System.Linq;
using System.Windows.Controls;
using WebView2.UserControls.ContextMenu;
using WebView2.ViewModels;

namespace WebView2.Services;

public class ContextMenuBuilder
{
    private static readonly string[] Allowed = {
        "undo", "redo", "back", "forward", "reload", "cut", "copy", "paste", "pasteAndMatchStyle", "inspectElement"
    };

    private readonly Microsoft.Web.WebView2.Wpf.WebView2 wv2;
    private readonly WebView2ViewModel vm;
    private readonly ContextMenu cm;

    public ContextMenuBuilder(Microsoft.Web.WebView2.Wpf.WebView2 webView2, WebView2ViewModel viewModel)
    {
        wv2 = webView2;
        vm = viewModel;
        cm = new ContextMenu();
    }

    public ContextMenuBuilder PopulateFromWebviewContextMenu(CoreWebView2ContextMenuRequestedEventArgs args)
    {
        object? last = null;

        foreach (var current in args.MenuItems) {
            switch (current.Kind) {
                case CoreWebView2ContextMenuItemKind.Submenu:
                case CoreWebView2ContextMenuItemKind.CheckBox:
                case CoreWebView2ContextMenuItemKind.Radio:
                    continue;
                case CoreWebView2ContextMenuItemKind.Separator: {
                    // avoid consecutive separators
                    if (last is not Separator) {
                        cm.Items.Add(last = new Separator());
                    }

                    continue;
                }
            }

            if (current.Name == "inspectElement" && vm.ContextMenu.CanInspectElement == false) {
                continue;
            }

            if (Allowed.Contains(current.Name) == false) {
                continue;
            }

            cm.Items.Add(last = ContextMenuFactory.From(args, current));

            if (current.Name == "reload") {
                cm.Items.Add(last = new ReloadWithoutCacheMenuItem(vm));
            }
        }

        return this;
    }

    public ContextMenuBuilder AddOpenInBrowser()
    {
        if (vm.ContextMenu.CanOpenInBrowser) {
            AddCustomMenuItem(new OpenInBrowserMenuItem(GetUrl()), "inspectElement");
        }

        return this;
    }

    public ContextMenuBuilder AddCopyUrl()
    {
        if (vm.ContextMenu.CanCopyUrl) {
            AddCustomMenuItem(new CopyUrlMenuItem(GetUrl()), "inspectElement");
        }

        return this;
    }
        
    public ContextMenuBuilder AddOpenTaskManager(CoreWebView2? webView2)
    {
        if (webView2 != null && vm.ContextMenu.CanOpenTaskManager) {
            AddCustomMenuItem(new OpenTaskManagerMenuItem(webView2), "inspectElement");
        }

        return this;
    }

    public ContextMenuBuilder AddSeparator()
    {
        if (vm.ContextMenu.CanCopyUrl || vm.ContextMenu.CanOpenInBrowser) {
            AddCustomMenuItem(new Separator(), "inspectElement");
        }

        return this;
    }

    public ContextMenu Build()
    {
        return cm;
    }

    private string GetUrl()
    {
        var uri = wv2.Source;
        if (vm.ContextMenu.IncludeSessionId) {
            UriBuilder builder = new(uri);
            builder.Query += (uri.Query.Contains('?') ? "&" : "?") + vm.Cookie.Name + "=" + vm.Cookie.Value;
            uri = builder.Uri;
        }

        return uri.ToString();
    }

    private void AddCustomMenuItem<T>(T newItem, string? beforeElementName) where T : Control
    {
        MenuItem? beforeElement = null;
        foreach (var item in cm.Items) {
            if (item is MenuItem menuItem && menuItem.Name == beforeElementName) {
                beforeElement = menuItem;
                break;
            }
        }

        if (beforeElement != null) {
            cm.Items.Insert(cm.Items.IndexOf(beforeElement), newItem);
        } else {
            cm.Items.Add(newItem);
        }
    }
}