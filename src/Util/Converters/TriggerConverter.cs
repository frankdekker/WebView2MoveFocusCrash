using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Util.Converters;

public class TriggerConverter : MarkupExtension, IMultiValueConverter
{
    public object Convert(object[]? values, Type? targetType, object? parameter, CultureInfo? culture)
    {
        // First value is target value.
        // All others are update triggers only.
        return values == null || values.Length < 1 ? Binding.DoNothing : values[0];
    }

    public object[] ConvertBack(object? value, Type[]? targetTypes, object? parameter, CultureInfo? culture)
    {
        throw new NotImplementedException();
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return this;
    }
}