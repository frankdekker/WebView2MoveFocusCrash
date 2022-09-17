using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace WebView2.Utility;

public static class HttpUtil
{
    public static string ToQueryString(Dictionary<string, string> parameters)
    {
        var escapedParams = parameters.Select(kvp =>
            string.Format("{0}={1}", HttpUtility.UrlEncode(kvp.Key, Encoding.UTF8), HttpUtility.UrlEncode(kvp.Value, Encoding.UTF8)));

        return string.Join("&", escapedParams);
    }
}