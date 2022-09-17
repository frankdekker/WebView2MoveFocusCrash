using System.IO;
using System.Text;

namespace WebView2.Utility;

public static class StreamUtil
{
    public static Stream ToStream(string data)
    {
        return new MemoryStream(new UTF8Encoding().GetBytes(data), false);
    }
}