namespace WebView2.ViewModels;

public class ContextMenuViewModel
{
    public bool IsEnabled { get; set; }
    public bool CanInspectElement { get; set; }
    public bool CanOpenTaskManager { get; set; }
    public bool CanOpenInBrowser { get; set; }
    public bool CanCopyUrl { get; set; }
    public bool IncludeSessionId { get; set; }
}