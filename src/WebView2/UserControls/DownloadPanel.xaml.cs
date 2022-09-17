#nullable enable
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WebView2.ViewModels;

namespace WebView2.UserControls;

public partial class DownloadPanel
{
    private ObservableCollection<DownloadItemViewModel>? downloads;

    public DownloadPanel()
    {
        InitializeComponent();
        DataContextChanged += OnDataContextChanged;
    }

    /// <summary>
    /// Register and Unregister collection change listener when data context is (un)assigned
    /// </summary>
    private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if (downloads != null) {
            downloads.CollectionChanged -= DownloadsOnCollectionChanged;
        }

        downloads = null;
        if (e.NewValue is ObservableCollection<DownloadItemViewModel> dl) {
            downloads = dl;
            downloads.CollectionChanged += DownloadsOnCollectionChanged;
        }
    }

    private void DownloadsOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        UpdateDownloadItemWidth();
    }

    /// <summary>
    /// When the size of the download panel changes, recalculate the widths of the download items
    /// </summary>
    protected override Size MeasureOverride(Size constraint)
    {
        var size = base.MeasureOverride(constraint);
        UpdateDownloadItemWidth();
        return size;
    }

    /// <summary>
    /// As there's no default layout manager that scales evenly, when available space shrinks, reduce the width of each download item
    /// based on the available space. With a min and max width preference for each download item 
    /// </summary>
    private void UpdateDownloadItemWidth()
    {
        var dl = downloads;
        if (dl == null) {
            return;
        }

        var count = dl.Count;
        if (count == 0) {
            return;
        }

        var availableWidth = DownloadItemList.ActualWidth;
        var preferredWidth = Math.Clamp(
            (int)Math.Floor(availableWidth / count),
            DownloadItemViewModel.ItemMinWidth,
            DownloadItemViewModel.ItemMaxWidth
        );

        DownloadItemList.HorizontalScrollBarVisibility =
            preferredWidth > DownloadItemViewModel.ItemMinWidth ? ScrollBarVisibility.Disabled : ScrollBarVisibility.Auto;

        foreach (var item in dl) {
            item.Width = preferredWidth;
        }
    }

    private void OnCloseDownloads(object sender, MouseButtonEventArgs e)
    {
        if (DataContext is IList collection) {
            collection.Clear();
        }
    }
}