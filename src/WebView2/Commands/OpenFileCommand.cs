#nullable enable
using Serilog;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Windows.Input;
using WebView2.ViewModels;

namespace WebView2.Commands;

public class OpenFileCommand : ICommand
{
    public bool CanExecute(object? parameter)
    {
        if (parameter is not DownloadItemViewModel download) {
            return false;
        }

        return download.Cancelled == false;
    }

    public void Execute(object? parameter)
    {
        if (parameter is not DownloadItemViewModel download || download.Cancelled) {
            return;
        }

        if (download.InProgress) {
            download.OpenOnCompletion = true;
        } else if (File.Exists(download.Filepath)) {
            Process.Start("explorer", download.Filepath);
        } else {
            Log.Warning("Unable to open download file. File does not exist: " + download.Filepath);
        }
    }

    [ExcludeFromCodeCoverage]
    public event EventHandler? CanExecuteChanged { add{} remove{} }
}