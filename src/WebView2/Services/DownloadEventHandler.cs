#nullable enable
using Serilog;
using Microsoft.Web.WebView2.Core;
using Microsoft.Win32;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using WebView2.ViewModels;

namespace WebView2.Services;

public class DownloadEventHandler
{
    private readonly ILogger log;
    private readonly UserControls.WebView2 webView2;

    public DownloadEventHandler(ILogger log, UserControls.WebView2 webView2)
    {
        this.log = log;
        this.webView2 = webView2;
    }

    public void OnDownloadStarted(object? sender, CoreWebView2DownloadStartingEventArgs args)
    {
        args.Handled = true;
        var                  vm       = this.webView2.ViewModel();
        CoreWebView2Deferral deferral = args.GetDeferral();
        SynchronizationContext.Current?.Post(_ =>
        {
            using (deferral) {
                if (vm == null) {
                    return;
                }

                int mode = vm.DownloadDialogCallback?.Invoke(args.ResultFilePath) ?? 2;
                if (mode == 0) {
                    args.Cancel = true;
                    return;
                }

                if (mode == 1) {
                    SaveFileDialog dlg = new() { FileName = Path.GetFileName(args.ResultFilePath) };
                    if (dlg.ShowDialog(Window.GetWindow(webView2)) == true) {
                        args.ResultFilePath = dlg.FileName;
                    } else {
                        args.Cancel = true;
                        return;
                    }
                }

                StartDownload(vm, args.DownloadOperation, mode);
            }
        }, null);
    }


    // Update download progress
    private void StartDownload(WebView2ViewModel vm, CoreWebView2DownloadOperation download, int mode)
    {
        log.Debug("Download starting: " + download.ResultFilePath);
            
        // create and queue download item
        var downloadItem = DownloadItemViewModelFactory.From(download, mode);
        vm.Downloads.Insert(0, downloadItem);
            
        download.EstimatedEndTimeChanged += delegate { downloadItem.CompletesAt = download.EstimatedEndTime; };
        download.BytesReceivedChanged += delegate { downloadItem.BytesReceived = download.BytesReceived; };
        download.StateChanged += delegate
        {
            downloadItem.InProgress = download.State == CoreWebView2DownloadState.InProgress;
            downloadItem.Cancelled = download.State == CoreWebView2DownloadState.Interrupted;
            downloadItem.Completed = download.State == CoreWebView2DownloadState.Completed;

            if (download.State != CoreWebView2DownloadState.Completed) {
                return;
            }

            log.Debug("Download completed: " + download.ResultFilePath);
                
            // download is completed
            downloadItem.BytesReceived = downloadItem.BytesTotal;
            downloadItem.Progress = 100;

            if (!downloadItem.OpenOnCompletion) {
                return;
            }

            // open file when download has completed
            downloadItem.OpenFileCommand.Execute(downloadItem);

            // remove download after 5 seconds
            Task.Run(async () =>
            {
                // wait 5 seconds
                await Task.Delay(5000);
                webView2.Dispatcher.Invoke(() => vm.Downloads.Remove(downloadItem));
            });
        };
    }
}