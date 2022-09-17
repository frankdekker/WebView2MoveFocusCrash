using System;

namespace Util.Utility;

public static class Asserts
{
    public static T NotNull<T>(T? value)
    {
        if (value == null) {
            throw new ArgumentNullException(nameof(value));
        }
        return value;
    }
        
    public static T IsType<T>(object? value)
    {
        try {
#pragma warning disable 8600
#pragma warning disable 8603
            return (T)value;
#pragma warning restore 8603
#pragma warning restore 8600
        } catch (Exception) {
            throw new ArgumentException(null, nameof(value));
        }
    }
}