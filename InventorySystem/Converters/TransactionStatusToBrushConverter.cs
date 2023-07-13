using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using InventorySystem.Models;

namespace InventorySystem.Converters;

public class TransactionStatusToBrushConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is TransactionStatus status)
        {
            return status switch
            {
                TransactionStatus.Pending => Brushes.LightGoldenrodYellow,
                TransactionStatus.Processed => Brushes.LightGreen,
                TransactionStatus.Failed => Brushes.Pink,
                TransactionStatus.Processing => Brushes.LightBlue,
                _ => throw new ArgumentOutOfRangeException(nameof(value), status, null)
            };
        }

        return DependencyProperty.UnsetValue;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
