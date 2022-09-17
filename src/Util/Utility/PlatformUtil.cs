using Serilog;
using System;
using System.Diagnostics;
using System.IO;

namespace Util.Utility;

public static class PlatformUtil
{
    public static void OpenInExplorer(string filepath)
    {
        if (Directory.Exists(filepath)) {
            Process.Start("explorer", "\"" + filepath + "\"");
        } else if (File.Exists(filepath)) {
            Process.Start("explorer", "/select, \"" + filepath + "\"");
        } else {
            Log.Warning("Unable to open download file. File does not exist: {FilePath}", filepath);
        }
    }
}