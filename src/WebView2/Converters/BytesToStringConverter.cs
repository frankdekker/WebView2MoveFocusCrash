using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace WebView2.Converters;

public class BytesToStringConverter : MarkupExtension, IValueConverter
{
    private const int GigaByte = 1073741824;
    private const int MegaByte = 1048576;
    private const int KiloByte = 1024;

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not long bytes) {
            return "0B";
        }

        String suffix;
        long   factor;

        switch (bytes) {
            case >= GigaByte:
                factor = GigaByte;
                suffix = "G";
                break;
            case >= MegaByte:
                factor = MegaByte;
                suffix = "M";
                break;
            default:
                factor = KiloByte;
                suffix = "K";
                break;
        }

        return $"{(double)bytes / factor:0.0}" + suffix;
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