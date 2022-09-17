using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace Util.Utility;

public static class BrowserUtil
{
    /// <summary>
    /// From stackoverflow: https://stackoverflow.com/questions/4580263/how-to-open-in-default-browser-in-c-sharp
    /// </summary>
    /// <param name="url"></param>
    public static void OpenInDefaultBrowser(string url)
    {
        try {
            Process.Start(url);
        } catch {
            // hack because of this: https://github.com/dotnet/corefx/issues/10361
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
                url = url.Replace("&", "^&");
                Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") {
                    CreateNoWindow = true, 
                    WindowStyle = ProcessWindowStyle.Hidden,
                    UseShellExecute = true
                });
            } else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {
                Process.Start("xdg-open", url);
            } else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) {
                Process.Start("open", url);
            } else {
                Log.Error("Failed to open url in default browser: " + url);
            }
        }
    }
}