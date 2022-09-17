using PropertyChanged;
using System;
using System.ComponentModel;
using System.Windows.Input;
using WebView2.Commands;

namespace WebView2.ViewModels;

[AddINotifyPropertyChangedInterface]
public class DownloadItemViewModel : INotifyPropertyChanged
{
    public const int ItemMinWidth = 41;
    public const int ItemMaxWidth = 171;
    private long bytesReceived = -1;
    private long bytesTotal = 0;

    public DownloadItemViewModel()
    {
        OpenFileCommand = new OpenFileCommand();
        OpenInFolderCommand = new OpenInFolderCommand();
    }

    public event PropertyChangedEventHandler PropertyChanged = (_, _) => { };

    public ICommand OpenFileCommand { get; set; }
    public ICommand OpenInFolderCommand { get; set; }
    public ICommand CancelCommand { get; set; }

    public string Filepath { get; set; }
    public string Name { get; set; }
    public int Progress { get; set; }


    public bool InProgress { get; set; }
    public bool Cancelled { get; set; }
    public bool Completed { get; set; }

    public int Width { get; set; } = ItemMaxWidth;

    public long BytesReceived
    {
        get => bytesReceived;
        set
        {
            bytesReceived = value;
            UpdateProgress();
        }
    }

    public long BytesTotal
    {
        get => bytesTotal;
        set
        {
            bytesTotal = value;
            UpdateProgress();
        }
    }

    public bool OpenOnCompletion { get; set; }
    public DateTime CompletesAt { get; set; }

    private void UpdateProgress()
    {
        if (bytesTotal <= 0 || bytesReceived <= 0) {
            Progress = 0;
            return;
        }
            
        Progress = Math.Clamp((int)(bytesReceived * 100 / bytesTotal), 0, 100);
    }
}