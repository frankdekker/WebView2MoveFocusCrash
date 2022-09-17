#nullable enable
using System;
using System.IO;
using System.Windows.Input;
using Util.Utility;

namespace WebView2.UserControls.ContextMenu;

public class OpenInExplorerMenuItem : ICommand
{
    private readonly string filepath;
#pragma warning disable 67
    public event EventHandler? CanExecuteChanged;
#pragma warning restore 67

    public OpenInExplorerMenuItem(string filepath)
    {
        this.filepath = filepath;
    }

    public bool CanExecute(object? parameter)
    {
        return File.Exists(filepath) || Directory.Exists(filepath);
    }

    public void Execute(object? parameter)
    {
        PlatformUtil.OpenInExplorer(filepath);
    }
}