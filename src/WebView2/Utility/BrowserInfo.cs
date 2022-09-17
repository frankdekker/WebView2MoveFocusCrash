#nullable enable
namespace WebView2.Utility;

public class BrowserInfo
{
    /// <summary>
    /// Available after atleast one WebView2 has been initialized
    /// </summary>
    public static string? WebView2Version { get; set; }
    
    /// <summary>
    /// Available after atleast one WebView2 has been initialized
    /// </summary>
    public static string? BrowserVersion { get; set; }
}