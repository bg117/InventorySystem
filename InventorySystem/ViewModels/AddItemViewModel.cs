using System.Windows.Input;
using InventorySystem.Common;
using InventorySystem.Models;

namespace InventorySystem.ViewModels;

public class AddItemViewModel : ObservableObject
{
    public InventorySingletonViewModel InventorySingletonInstance => InventorySingletonViewModel.Instance;

    private string _name;
    public string Name
    {
        get => _name;
        set => SetField(ref _name, value);
    }

    private string _description;
    public string Description
    {
        get => _description;
        set => SetField(ref _description, value);
    }

    private int _quantity = 1;
    public int Quantity
    {
        get => _quantity;
        set => SetField(ref _quantity, value);
    }

    private decimal _price;
    public decimal Price
    {
        get => _price;
        set => SetField(ref _price, value);
    }

    private ICommand _addItemCommand;

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
        InventorySingletonInstance.Items.Add(item);
    }

    private bool CanAddItem()
    {
        return !string.IsNullOrWhiteSpace(Name);
    }
}
