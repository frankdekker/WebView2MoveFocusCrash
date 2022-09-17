using System;
using System.Runtime.InteropServices;

namespace WebView2.Utility;

public static class WebviewFocus
{
    private const uint GW_CHILD = 5;
    [DllImport("user32.dll")]
    private static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);
    [DllImport("user32.dll")]
    private static extern IntPtr SetFocus(IntPtr hWnd);

    /**
     * WebView2 focus is not exactly working. Focus the webview window via window native function
     *
     * @link https://stackoverflow.com/a/66259582/10026704
     * @link https://github.com/MicrosoftEdge/WebView2Feedback/issues/686
     */
    public static void Focus(Microsoft.Web.WebView2.Wpf.WebView2 webview)
    {
        SetFocus(GetWindow(webview.Handle, GW_CHILD));
    }        
}