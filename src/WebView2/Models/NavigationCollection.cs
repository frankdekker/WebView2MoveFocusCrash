using System;
using System.Collections.Generic;

namespace WebView2.Models;

/// <summary>
///  Class to keep track of how many navigation (frames) are currently loading.
/// </summary>
public class NavigationCollection
{
    public event EventHandler NavigationCount;
    private readonly Dictionary<ulong, bool> dictionary;

    public NavigationCollection()
    {
        dictionary = new Dictionary<ulong, bool>(5);
    }

    public void Start(ulong navigationId)
    {
        // occasionally frames with the same navigation id are started. Prevent this collision.
        if (dictionary.ContainsKey(navigationId) == false) {
            dictionary.Add(navigationId, true);
            OnNavigationCountChange(EventArgs.Empty);
        }
    }

    public void Finish(ulong navigationId)
    {
        dictionary.Remove(navigationId, out _);
        OnNavigationCountChange(EventArgs.Empty);
    }

    public int Count => dictionary.Count;

    private void OnNavigationCountChange(EventArgs e)
    {
        EventHandler handler = NavigationCount;
        handler?.Invoke(this, e);
    }
}