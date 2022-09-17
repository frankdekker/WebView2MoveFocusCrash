using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using WebView2.Actions;
using WebView2.Models;

namespace WebView2.ViewModels;

[ExcludeFromCodeCoverage]
[AddINotifyPropertyChangedInterface]
public class WebView2ViewModel : INotifyPropertyChanged, IDisposable
{
    public event PropertyChangedEventHandler PropertyChanged = (_, _) => { };

    public delegate void AlertDialogDelegate(string message);

    public delegate bool ConfirmDialogDelegate(string message);
        
    /// <summary>
    /// 2 = open, 1 = save, 0 = cancel
    /// </summary>
    public delegate int DownloadDialogDelegate(string filePath);

    public delegate void FocusRequestDelegate(Microsoft.Web.WebView2.Wpf.WebView2 webView2);
        
    public delegate bool NewWindowDelegate(NewWindowEvent windowEvent);
        
    public delegate void ResponseHeadersDelegate(IEnumerable<KeyValuePair<string, string>> headers);

    public string UserDataFolder { get; set; }

    public ICookie Cookie { get; set; }

    public string Source { get; set; }

    public Dictionary<string, string> PostData { get; set; }

    public IHostViewModel HostModel { get; set; }

    public AlertDialogDelegate AlertDialogCallback { get; set; }
    public ConfirmDialogDelegate ConfirmDialogCallback { get; set; }
    public DownloadDialogDelegate DownloadDialogCallback { get; set; }
    public NewWindowDelegate NewWindowCallback { get; set; }
    public FocusRequestDelegate FocusRequestCallback { get; set; }
    public ResponseHeadersDelegate ResponseHeadersCallback { get; set; }
    public ContextMenuViewModel ContextMenu { get; } = new();
    public ToggleXDebugAction ToggleXDebug { get; set; }
    public FocusAction Focus { get; set; }
    public ReloadAction Reload { get; set; }
    public bool Loading { get; set; }

    /// <summary>
    /// Keep track and show currently active downloads
    /// </summary>
    public ObservableCollection<DownloadItemViewModel> Downloads { get; set; } = new();
        
    public void Dispose()
    {
        Cookie = null;
        Source = null;
        Downloads.Clear();
        HostModel?.Dispose();
        HostModel = null;
    }
}