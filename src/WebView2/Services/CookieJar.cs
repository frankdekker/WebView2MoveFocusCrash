using System;
using System.Collections.Generic;
using WebView2.Models;
using MSWebView2 = Microsoft.Web.WebView2.Wpf.WebView2;

namespace WebView2.Services;

public class CookieJar
{
    private MSWebView2 webView;

    public CookieJar(MSWebView2 webView)
    {
        this.webView = webView;
    }

    public void SetCookie(ICookie cookie)
    {
        if (this.webView.IsInitialized == false) {
            return;
        }

        cookie.Domains.ForEach(domain =>
        {
            var webviewCookie = this.webView.CoreWebView2.CookieManager.CreateCookie(cookie.Name, cookie.Value, domain, cookie.Path);
            webviewCookie.IsHttpOnly = cookie.HttpOnly;
            webviewCookie.IsSecure = cookie.Secure;
            webviewCookie.Expires = DateTimeOffset.FromUnixTimeSeconds(cookie.Expires).UtcDateTime;
            this.webView.CoreWebView2.CookieManager.AddOrUpdateCookie(webviewCookie);
        });
    }


    public void SetCookie(
        string name,
        string value,
        List<String> domains,
        bool isSecure = false,
        bool isHttpOnly = false,
        DateTime? expires = null
    )
    {
        if (this.webView.IsInitialized == false) {
            return;
        }

        domains.ForEach(domain =>
        {
            var cookie = this.webView.CoreWebView2.CookieManager.CreateCookie(name, value, domain, "/");
            cookie.IsSecure = isSecure;
            cookie.IsHttpOnly = isHttpOnly;
            cookie.Expires = expires ?? DateTime.Now.AddYears(1);
            this.webView.CoreWebView2.CookieManager.AddOrUpdateCookie(cookie);
        });
    }

    public void DeleteCookie(string name, List<String> domains)
    {
        if (this.webView.IsInitialized == false) {
            return;
        }

        domains.ForEach(domain =>
        {
            var cookie = this.webView.CoreWebView2.CookieManager.CreateCookie(name, "", domain, "/");
            this.webView.CoreWebView2.CookieManager.DeleteCookie(cookie);
        });
    }
}