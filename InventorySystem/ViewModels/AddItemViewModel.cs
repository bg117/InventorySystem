using System.Windows.Input;
using InventorySystem.Common;
using InventorySystem.Models;

namespace InventorySystem.ViewModels;

public class AddItemViewModel : ViewModelBase
{
    private ICommand _addItemCommand;

    private string _description;

    private string _name;

    private decimal _price;

    private int _quantity = 1;

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

    public ICommand AddItemCommand => _addItemCommand ??= new RelayCommand(AddItem, CanAddItem);

    private void AddItem()
    {
        var item = new Item
        {
            Id = Item.NewId(),
            Name = Name,
            Description = Description,
            Quantity = Quantity,
            Price = Price
        };

        InventorySingletonViewModel.Instance.Items.Add(item);
    }

    private bool CanAddItem()
    {
        return !string.IsNullOrWhiteSpace(Name);
    }
}