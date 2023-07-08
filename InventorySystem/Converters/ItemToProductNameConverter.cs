using System;
using System.Globalization;
using System.Windows.Data;
using InventorySystem.Models;

namespace InventorySystem.Converters;

public class ItemToProductNameConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not Item item)
            throw new ArgumentException($"{nameof(value)} must be of type {typeof(Item)}");

        var baseName = $"{item.Id} ({item.Name})";
        if (!string.IsNullOrEmpty(item.Description))
            baseName += " - " + item.Description;

        return baseName;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}