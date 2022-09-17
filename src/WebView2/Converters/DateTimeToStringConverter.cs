using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace WebView2.Converters;

/// <summary>
/// Transform DateTime to String converter
/// </summary>
public class DateTimeToStringConverter : MarkupExtension, IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not DateTime date) {
            return "Unknown";
        }

        double seconds = (date - DateTime.Now).TotalSeconds;
        string suffix;
        double factor;

        if (seconds >= 3600) {
            suffix = "Hours";
            factor = 3600;
        } else if (seconds >= 60) {
            suffix = "minutes";
            factor = 60;
        } else {
            suffix = "seconds";
            factor = 1;
        }

        return $"{seconds / factor:0.#} " + suffix;
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