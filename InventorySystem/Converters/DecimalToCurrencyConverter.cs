using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace InventorySystem.Converters;

public class DecimalToCurrencyConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not decimal decimalValue)
            throw new ArgumentException("Value must be decimal", nameof(value));
        
        return decimalValue.ToString("C", culture);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not string stringValue)
            throw new ArgumentException("Value must be string", nameof(value));
        
        return decimal.Parse(stringValue, NumberStyles.Currency, culture);
    }
}