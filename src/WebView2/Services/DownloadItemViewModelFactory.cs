using Microsoft.Web.WebView2.Core;
using System.IO;
using WebView2.Commands;
using WebView2.ViewModels;

namespace WebView2.Services;

public static class DownloadItemViewModelFactory
{
    public static DownloadItemViewModel From(CoreWebView2DownloadOperation download, int mode)
    {
        return new DownloadItemViewModel {
            CancelCommand = new CancelCommand(download.Cancel),
            Name = Path.GetFileName(download.ResultFilePath),
            Filepath = download.ResultFilePath,
            InProgress = download.State == CoreWebView2DownloadState.InProgress,
            Cancelled = download.State == CoreWebView2DownloadState.Interrupted,
            Completed = download.State == CoreWebView2DownloadState.Completed,
            CompletesAt = download.EstimatedEndTime,
            BytesReceived = download.BytesReceived,
            BytesTotal = (long)(download.TotalBytesToReceive ?? 0),
            OpenOnCompletion = mode == 2,
        };
    }
}