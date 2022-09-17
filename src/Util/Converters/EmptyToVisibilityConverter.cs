﻿using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Util.Converters;

public class EmptyToVisibilityConverter : MarkupExtension, IValueConverter
{
    public object Convert(object? value, Type? targetType, object? parameter, CultureInfo? culture)
    {
        if (value == null) {
            return Visibility.Collapsed;
        }

        if (value is int intValue) {
            return intValue == 0 ? Visibility.Collapsed : Visibility.Visible;
        }

        if (value is ICollection collection) {
            return collection.Count == 0 ? Visibility.Collapsed : Visibility.Visible;
        }

        if (value is string stringValue) {
            return stringValue.Length == 0 ? Visibility.Collapsed : Visibility.Visible;
        }

        return Visibility.Visible;
    }

    [ExcludeFromCodeCoverage]
    public object ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo? culture)
    {
        throw new NotImplementedException();
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return this;
    }
}