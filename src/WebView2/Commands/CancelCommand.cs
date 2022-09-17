#nullable enable
using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;
using WebView2.ViewModels;

namespace WebView2.Commands;

public class CancelCommand : ICommand
{
    private readonly CancelAction cancelAction;

    public delegate void CancelAction();

    public CancelCommand(CancelAction cancelAction)
    {
        this.cancelAction = cancelAction;
    }
        
    public bool CanExecute(object? parameter)
    {
        return parameter is DownloadItemViewModel { InProgress: true };
    }

    public void Execute(object? parameter)
    {
        if (parameter is not DownloadItemViewModel download) {
            return;
        }

        download.Cancelled = true;
        this.cancelAction.Invoke();
    }

    [ExcludeFromCodeCoverage]
    public event EventHandler? CanExecuteChanged { add{} remove{} }
}