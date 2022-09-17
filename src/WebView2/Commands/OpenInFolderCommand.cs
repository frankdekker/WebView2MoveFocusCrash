#nullable enable
using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;
using Util.Utility;
using WebView2.ViewModels;

namespace WebView2.Commands;

public class OpenInFolderCommand : ICommand
{
    public bool CanExecute(object? parameter)
    {
        if (parameter is not DownloadItemViewModel download) {
            return false;
        }

        return download.Cancelled == false;
    }

    [ExcludeFromCodeCoverage]
    public void Execute(object? parameter)
    {
        if (parameter is not DownloadItemViewModel download || download.Cancelled) {
            return;
        }

        PlatformUtil.OpenInExplorer(download.Filepath);
    }

    [ExcludeFromCodeCoverage]
    public event EventHandler? CanExecuteChanged { add{} remove{} }
}