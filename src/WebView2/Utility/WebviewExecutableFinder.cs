#nullable enable
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace WebView2.Utility;

public static class WebviewExecutableFinder
{
    public static string? GetExecutablePath()
    {
        var regex       = new Regex(@"\\(\d+\.\d+\.\d+\.\d+)$");
        var path        = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\AppData\Local\Microsoft\Edge SxS\Application";
        
        // verify directory exists
        if (Directory.Exists(path) == false) {
            return null;
        }
        
        WebviewBinaryVersion? bestVersion   = null;
        string?               bestDirectory = null;

        foreach (string directory in Directory.GetDirectories(path)) {
            var matches = regex.Match(directory);
            if (matches.Success == false) {
                continue;
            }

            var version = WebviewBinaryVersion.Create(matches.Groups[1].Value);
            if (bestVersion != null && bestVersion.IsNewer(version)) {
                continue;
            }

            bestVersion = version;
            bestDirectory = directory;
        }

        return bestDirectory;
    }
}