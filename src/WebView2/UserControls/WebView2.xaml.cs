#nullable enable
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;
using Serilog;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Threading;
using System.Windows;
using Util.Threads;
using WebView2.Models;
using WebView2.Services;
using WebView2.Utility;
using WebView2.ViewModels;

namespace WebView2.UserControls;

/// <summary>
/// Interaction logic for WebView2.xaml
/// </summary>
[ExcludeFromCodeCoverage]
public partial class WebView2
{
    private readonly NavigationCollection navigationCollection = new();
    private readonly DownloadEventHandler downloadHandler;
    private WebView2ViewModel? viewModel;
    private CancellationTokenSource? cancelUnloadToken;
    private CookieJar? cookieJar;

    public WebView2()
    {
        InitializeComponent();
        DataContextChanged += UserControl_DataContextChanged;
        navigationCollection.NavigationCount += NavigationCollection_NavigationCount;
        WebView.CoreWebView2InitializationCompleted += WebView2_CoreWebView2Ready;
        downloadHandler = new DownloadEventHandler(Log.Logger, this);
        Unloaded += WebView2_Unloaded;
        Loaded += WebView2_loaded;
    }

    public WebView2ViewModel? ViewModel()
    {
        return viewModel;
    }

    private void WebView2_loaded(object sender, RoutedEventArgs e)
    {
        cancelUnloadToken?.Cancel();
        cancelUnloadToken = null;
    }

    private void WebView2_Unloaded(object sender, RoutedEventArgs e)
    {
        // delay dispose slightly as dpi changes and tab drag&drop also trigger unload
        cancelUnloadToken = Scheduler.Delay(2500, this.DisposeView, this.Dispatcher);
    }

    private void DisposeView()
    {
        DataContextChanged -= UserControl_DataContextChanged;
        navigationCollection.NavigationCount -= NavigationCollection_NavigationCount;
        WebView.CoreWebView2InitializationCompleted -= WebView2_CoreWebView2Ready;
        WebView.CoreWebView2.ContextMenuRequested -= WebView_ContextMenuRequested;
        WebView.CoreWebView2.NewWindowRequested -= WebView2_NewWindowRequested;
        WebView.CoreWebView2.FrameNavigationStarting -= WebView_NavigationStarting;
        WebView.CoreWebView2.FrameNavigationCompleted -= WebView_NavigationCompleted;
        WebView.CoreWebView2.NavigationStarting -= WebView_NavigationStarting;
        WebView.CoreWebView2.NavigationCompleted -= WebView_NavigationCompleted;
        WebView.CoreWebView2.ScriptDialogOpening -= WebView2_ScriptDialogOpening;
        WebView.CoreWebView2.WebResourceResponseReceived -= WebView2_WebResourceResponseReceived;
        WebView.CoreWebView2.DownloadStarting -= this.downloadHandler.OnDownloadStarted;
        WebView.CoreWebView2.ProcessFailed -= OnProcessFailed;
        WebView.CoreWebView2.Environment.BrowserProcessExited -= OnBrowserProcessExited;

        Unloaded -= WebView2_Unloaded;
        Loaded -= WebView2_loaded;
        viewModel = null;
        cookieJar = null;
        cancelUnloadToken = null;

        Log.Debug("Disposing WebView2");
        try {
            WebView.Dispose();
        } catch (Exception err) {
            Log.Error(err, "Failed to dispose WebView: {Error}", err.Message);
        }
    }

    private void WebView2_CoreWebView2Ready(object? sender, EventArgs e)
    {
        if (WebView.CoreWebView2 == null) {
            Log.Error("CoreWebView2Ready event was send, but CoreWebView2 was null. Crash?");
            viewModel?.AlertDialogCallback?.Invoke("Browser crashed");
            return;
        }

        cookieJar = new CookieJar(WebView);

        // Inject session cookie into the browser
        if (viewModel != null) {
            if (viewModel.Cookie != null) {
                cookieJar.SetCookie(viewModel.Cookie);
            }

            if (viewModel.HostModel != null) {
                WebView.CoreWebView2.AddHostObjectToScript(viewModel.HostModel.GetName(), viewModel.HostModel);
            }

            try {
                WebView.Focus();
            } catch (Exception err) {
                Log.Information(err, "Failed to request focus for WebView: {Error}", err.Message);
            }
        }

        this.WebView.CoreWebView2.Settings.AreHostObjectsAllowed = true;
        this.WebView.CoreWebView2.Settings.AreDefaultContextMenusEnabled = true;
        this.WebView.CoreWebView2.Settings.AreDefaultScriptDialogsEnabled = false;
        this.WebView.CoreWebView2.Settings.IsStatusBarEnabled = false;
        this.WebView.CoreWebView2.Settings.IsGeneralAutofillEnabled = false;
        this.WebView.CoreWebView2.Settings.IsPasswordAutosaveEnabled = false;
        this.WebView.CoreWebView2.Settings.IsPinchZoomEnabled = false;
        this.WebView.CoreWebView2.Settings.IsSwipeNavigationEnabled = false;
        this.WebView.CoreWebView2.Settings.IsZoomControlEnabled = true;
        this.WebView.CoreWebView2.Settings.HiddenPdfToolbarItems = CoreWebView2PdfToolbarItems.None;

        this.WebView.CoreWebView2.ContextMenuRequested += WebView_ContextMenuRequested;
        this.WebView.CoreWebView2.FrameNavigationStarting += WebView_NavigationStarting;
        this.WebView.CoreWebView2.FrameNavigationCompleted += WebView_NavigationCompleted;
        this.WebView.CoreWebView2.NavigationStarting += WebView_NavigationStarting;
        this.WebView.CoreWebView2.NavigationCompleted += WebView_NavigationCompleted;
        this.WebView.CoreWebView2.NewWindowRequested += WebView2_NewWindowRequested;
        this.WebView.CoreWebView2.ScriptDialogOpening += WebView2_ScriptDialogOpening;
        this.WebView.CoreWebView2.WebResourceResponseReceived += WebView2_WebResourceResponseReceived;
        this.WebView.CoreWebView2.DownloadStarting += this.downloadHandler.OnDownloadStarted;
        this.WebView.CoreWebView2.ProcessFailed += OnProcessFailed;
        this.WebView.CoreWebView2.Environment.BrowserProcessExited += OnBrowserProcessExited;
    }

    private static void OnBrowserProcessExited(object? sender, CoreWebView2BrowserProcessExitedEventArgs e)
    {
        if (e.BrowserProcessExitKind == CoreWebView2BrowserProcessExitKind.Failed) {
            Log.Error("Browser process terminated: {ProcessId}", e.BrowserProcessId);
        }
    }
    
    private void OnProcessFailed(object? sender, CoreWebView2ProcessFailedEventArgs e)
    {
        Log.Error("Browser crashed with: {Kind}: {Reason}", e.ProcessFailedKind, e.Reason);
    }

    private void WebView_ContextMenuRequested(object? sender, CoreWebView2ContextMenuRequestedEventArgs args)
    {
        args.Handled = true;
        if (DataContext is not WebView2ViewModel vm || vm.ContextMenu.IsEnabled == false) {
            return;
        }

        // construct context menu based on the browser suggestion
        var cm = new ContextMenuBuilder(this.WebView, vm)
            .PopulateFromWebviewContextMenu(args)
            .AddOpenInBrowser()
            .AddCopyUrl()
            .AddOpenTaskManager(this.WebView.CoreWebView2)
            .AddSeparator()
            .Build();

        var deferral = args.GetDeferral();
        cm.Closed += (_, _) => deferral.Complete();
        cm.IsOpen = true;
    }

    private void WebView2_WebResourceResponseReceived(object? sender, CoreWebView2WebResourceResponseReceivedEventArgs e)
    {
        viewModel?.ResponseHeadersCallback?.Invoke(e.Response.Headers);
    }

    private void WebView2_ScriptDialogOpening(object? sender, CoreWebView2ScriptDialogOpeningEventArgs e)
    {
        if (viewModel == null) {
            return;
        }

        if (e.Kind == CoreWebView2ScriptDialogKind.Alert && viewModel.AlertDialogCallback != null) {
            viewModel.AlertDialogCallback(e.Message);
            // dialog will take focus and not give it back, retake focus when dialog was closed
            Scheduler.Delay(50, () => WebviewFocus.Focus(this.WebView), this.WebView.Dispatcher);
        }

        if (e.Kind == CoreWebView2ScriptDialogKind.Confirm && viewModel.ConfirmDialogCallback != null &&
            viewModel.ConfirmDialogCallback(e.Message)) {
            e.Accept();
            // dialog will take focus and not give it back, retake focus when dialog was closed
            Scheduler.Delay(50, () => WebviewFocus.Focus(this.WebView), this.WebView.Dispatcher);
        }
    }

    private void WebView2_NewWindowRequested(object? sender, CoreWebView2NewWindowRequestedEventArgs e)
    {
        if (viewModel?.NewWindowCallback == null) {
            return;
        }

        var windowEvent = new NewWindowEvent(e.Uri);
        if (e.WindowFeatures.HasPosition) {
            windowEvent.Top = (int)e.WindowFeatures.Top;
            windowEvent.Left = (int)e.WindowFeatures.Left;
        }

        if (e.WindowFeatures.HasSize) {
            windowEvent.Width = (int)e.WindowFeatures.Width;
            windowEvent.Height = (int)e.WindowFeatures.Height;
        }

        e.Handled = viewModel.NewWindowCallback(windowEvent);
        Log.Debug("new window requested: {Uri}, Handled: {Handled}", e.Uri, e.Handled ? "true" : "false");
    }

    private void WebView_NavigationStarting(object? sender, CoreWebView2NavigationStartingEventArgs e)
    {
        navigationCollection.Start(e.NavigationId);
    }

    private void WebView_NavigationCompleted(object? sender, CoreWebView2NavigationCompletedEventArgs e)
    {
        navigationCollection.Finish(e.NavigationId);
    }

    private void NavigationCollection_NavigationCount(object? sender, EventArgs e)
    {
        if (viewModel != null) {
            viewModel.Loading = navigationCollection.Count > 0;
        }
    }

    private void UserControl_DataContextChanged(object? s, DependencyPropertyChangedEventArgs e)
    {
        if (e.NewValue is WebView2ViewModel model) {
            viewModel = model;
        } else {
            viewModel = null;
            return;
        }

        viewModel.Reload = async () =>
        {
            Log.Debug("Reloading: {Uri}", this.WebView.Source.AbsoluteUri);
            try {
                await this.WebView.CoreWebView2.Profile.ClearBrowsingDataAsync(CoreWebView2BrowsingDataKinds.DiskCache);
            } catch (Exception) {
                // ignore errors when feature is not available
            }

            Application.Current.Dispatcher.Invoke(() => this.WebView.Reload());
        };

        viewModel.Focus = () =>
        {
            if (this.WebView.CoreWebView2 != null) {
                Log.Debug("Focusing: {Uri}", this.WebView.Source.AbsoluteUri);
                this.WebView.Focus();
                // allow view model to specify callback on focus
                viewModel.FocusRequestCallback?.Invoke(this.WebView);
            }
        };

        // listen for xdebug toggles                    
        viewModel.ToggleXDebug = (domains, isEnabled) =>
        {
            if (this.WebView.CoreWebView2 == null || domains == null || cookieJar == null) {
                return;
            }

            if (isEnabled) {
                cookieJar.SetCookie("XDEBUG_SESSION", "PHPSTORM", domains);
            } else {
                cookieJar.DeleteCookie("XDEBUG_SESSION", domains);
            }
        };

        // configure language and user data folder
        var properties = new CoreWebView2CreationProperties { Language = CultureInfo.DefaultThreadCurrentUICulture?.ToString() ?? "en-US" };
        if (viewModel.UserDataFolder != null) {
            var executablePath = WebviewExecutableFinder.GetExecutablePath();
            if (executablePath != null) {
                properties.BrowserExecutableFolder = executablePath;
            }

            properties.UserDataFolder = viewModel.UserDataFolder + properties.Language;
        }

        this.WebView.CreationProperties = properties;

        // if source is url, load page otherwise set source as string content
        var opener = new WebViewOpener(this.WebView);
        if (viewModel.Source.StartsWith("http") == false) {
            opener.OpenContent(viewModel.Source);
        } else if (viewModel.PostData is { Count: > 0 }) {
            opener.PostUrl(viewModel.Source, viewModel.PostData);
        } else {
            opener.GetUrl(viewModel.Source);
        }
    }
}