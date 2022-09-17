using System.Collections.ObjectModel;
using System.Windows.Input;

namespace WebViewExample.ViewModels;

public class MainViewModel
{
    public ObservableCollection<BrowserTab> ItemCollection { get; set; } = new();
    public BrowserTab? SelectedTab { get; set; }

    public ICommand? CloseTabCommand { get; set; }
    
    
    // public WebView2ViewModel? WebView2ViewModel { get; set; }
}