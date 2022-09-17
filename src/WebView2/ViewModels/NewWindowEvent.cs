namespace WebView2.ViewModels;

public class NewWindowEvent
{
    public NewWindowEvent(string uri)
    {
        Uri = uri;
    }
        
    public int? Top { get; set; }
    public int? Left { get; set; }
    public int? Width { get; set; }
    public int? Height { get; set; }
    public string Uri { get; }
}