using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using InventorySystem.Models;
using InventorySystem.ViewModels;

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
        if (value is not string combination)
            throw new ArgumentException($"{nameof(value)} must be of type {typeof(string)}");

        var split = combination.Split();
        var id = System.Convert.ToInt32(split[0]); // ID of item

        return InventorySingletonViewModel.Instance.Items.FirstOrDefault(item => item.Id == id) ??
               throw new ArgumentException(
                   $"There does not exist an element of type {typeof(Item)} with {nameof(Item.Id)} == {id}");
    }
}