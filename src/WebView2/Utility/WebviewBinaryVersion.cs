using System;
using System.Data;
using System.Text.RegularExpressions;

namespace WebView2.Utility;

public class WebviewBinaryVersion
{
    public readonly int Major;
    public readonly int Minor;
    public readonly int Patch;
    public readonly int Build;

    public WebviewBinaryVersion(int major, int minor, int patch, int build)
    {
        this.Major = major;
        this.Minor = minor;
        this.Patch = patch;
        this.Build = build;
    }

    public bool IsNewer(WebviewBinaryVersion version)
    {
        if (version.Major != Major) {
            return version.Major < Major;
        }

        if (version.Minor != Minor) {
            return version.Minor < Minor;
        }

        if (version.Patch != Patch) {
            return version.Patch < Patch;
        }

        return version.Build < Build;
    }

    public static WebviewBinaryVersion Create(string version)
    {
        var regex   = new Regex(@"^(\d+)\.(\d+)\.(\d+)\.(\d+)$");
        var matches = regex.Match(version);
        if (matches.Success == false) {
            throw new InvalidConstraintException("Invalid version: " + version);
        }

        return new WebviewBinaryVersion(
            Int32.Parse(matches.Groups[1].Value),
            Int32.Parse(matches.Groups[2].Value),
            Int32.Parse(matches.Groups[3].Value),
            Int32.Parse(matches.Groups[4].Value)
        );
    }
}