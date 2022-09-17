using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace WebView2.Converters;

/// <summary>
/// Transform 0-100% progress into a 0-360 degree angle
/// </summary>
public class ProgressToAngleConverter : MarkupExtension, IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if ((value is int) == false) {
            return 0;
        }

        return (int)(Math.Max(Math.Min((int)value, 100), 0) * 3.6);
    }

    [ExcludeFromCodeCoverage]
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return this;
    }
}