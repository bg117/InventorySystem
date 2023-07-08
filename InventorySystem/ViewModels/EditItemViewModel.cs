using System.Windows.Input;
using InventorySystem.Common;
using InventorySystem.Models;

namespace InventorySystem.ViewModels;

public class EditItemViewModel : ViewModelBase
{
    private string _description;
    private Item _item = new();

    private string _name;

    private decimal _price;

    private int _quantity;

    private ICommand _updateItemCommand;

    public Item Item
    {
        get => _item;
        set
        {
            SetField(ref _item, value);
            Name = Item.Name;
            Description = Item.Description;
            Quantity = Item.Quantity;
            Price = Item.Price;
        }
    }

    public string Name
    {
        get => _name;
        set => SetField(ref _name, value);
    }

    public string Description
    {
        get => _description;
        set => SetField(ref _description, value);
    }

    public int Quantity
    {
        get => _quantity;
        set => SetField(ref _quantity, value);
    }

    public decimal Price
    {
        get => _price;
        set => SetField(ref _price, value);
    }

    public ICommand UpdateItemCommand => _updateItemCommand ??= new RelayCommand(UpdateItem, CanUpdateItem);

    private void UpdateItem()
    {
        var idx = InventorySingletonViewModel.Instance.Items.IndexOf(Item);
        InventorySingletonViewModel.Instance.Items[idx] = new Item
        {
            Id = Item.Id,
            Name = Name,
            Description = Description,
            Quantity = Quantity,
            Price = Price
        };
    }

    private bool CanUpdateItem()
    {
        return !string.IsNullOrWhiteSpace(Name);
    }
}