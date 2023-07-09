using System;
using System.Globalization;
using System.Windows.Data;
using OfficeOpenXml;
using OfficeOpenXml.Table;

namespace InventorySystem.Converters;

public class ExcelTableToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not ExcelTable table)
            throw new ArgumentException($"{nameof(value)} must be of type {typeof(ExcelRange)}");

        var name = !string.IsNullOrEmpty(table.Name) ? table.Name : "Unnamed Table";
        var address = table.Address.Address;

        return $"{name} ({address})";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}