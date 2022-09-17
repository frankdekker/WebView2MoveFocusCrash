using WebView2.ViewModels;

namespace WebViewExample.ViewModels;

public class BrowserTab
{
    public string TabName { get; set; } = "TabName";

    public WebView2ViewModel? WebView2ViewModel { get; set; }
}