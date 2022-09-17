using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Util.Converters;

public class BoolToVisibilityConverter : MarkupExtension, IValueConverter
{
    private const string Negate = "Negate";

    public object Convert(object value, Type? targetType, object? parameter, CultureInfo? culture)
    {
        if ((value is bool) == false) {
            return Visibility.Collapsed;
        }

        if (parameter is Negate) {
            value = !(bool)value;
        }

        return (bool)value ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo? culture)
    {
        return value is Visibility.Visible;
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return this;
    }
}