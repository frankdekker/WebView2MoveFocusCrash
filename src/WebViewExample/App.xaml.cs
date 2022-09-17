using System.Linq;
using System.Windows;
using Util.Commands;
using WebView2.ViewModels;
using WebViewExample.ViewModels;

namespace WebViewExample
{
    public partial class App
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            var tab1 = new BrowserTab {
                TabName = "Wikipedia", WebView2ViewModel = new WebView2ViewModel { Source = "https://en.wikipedia.org/wiki/Main_Page" }
            };
            var tab2 = new BrowserTab { TabName = "Bing", WebView2ViewModel = new WebView2ViewModel { Source = "https://www.bing.com" } };
            var tab3 = new BrowserTab {
                TabName = "Github", WebView2ViewModel = new WebView2ViewModel { Source = "https://github.com/MicrosoftEdge/WebView2Feedback" }
            };

            var vm = new MainViewModel { SelectedTab = tab1 };
            vm.CloseTabCommand = new RelayCommand(tab =>
            {
                if (tab is BrowserTab browserTab) {
                    vm.ItemCollection.Remove(browserTab);
                    if (vm.SelectedTab == browserTab) {
                        vm.SelectedTab = vm.ItemCollection.FirstOrDefault();
                    }
                }
            });
            vm.ItemCollection.Add(tab1);
            vm.ItemCollection.Add(tab2);
            vm.ItemCollection.Add(tab3);
            var window = new MainWindow { DataContext = vm };
            window.Show();
            window.Activate();
        }
    }
}