using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace WebView2.Utility;

/// <summary>
/// Class to open html content, GET or POST url in webview
/// </summary>
[ExcludeFromCodeCoverage] 
public class WebViewOpener
{
    private readonly Microsoft.Web.WebView2.Wpf.WebView2 webView2;

    public WebViewOpener(Microsoft.Web.WebView2.Wpf.WebView2 webView2)
    {
        this.webView2 = webView2;
    }

    public void OpenContent(string content)
    {
        webView2.Dispatcher.Invoke(async () =>
        {
            await webView2.EnsureCoreWebView2Async().ConfigureAwait(true);
            webView2.CoreWebView2.NavigateToString(content);
        });
    }

    public void GetUrl(string url)
    {
        Log.Debug("Opening url via GET: " + url);
        webView2.Source = new Uri(url);
    }

    public void PostUrl(string url, Dictionary<string, string> postData)
    {
        webView2.Dispatcher.Invoke(async () =>
        {
            var queryString = HttpUtil.ToQueryString(postData);
            Log.Debug("Opening url via POST: " + url + ". DATA: " + queryString);

            await webView2.EnsureCoreWebView2Async().ConfigureAwait(true);
            var request = webView2.CoreWebView2.Environment.CreateWebResourceRequest(
                url,
                "POST",
                StreamUtil.ToStream(queryString),
                "Content-Type: application/x-www-form-urlencoded"
            );
            webView2.CoreWebView2.NavigateWithWebResourceRequest(request);
        });
    }
}