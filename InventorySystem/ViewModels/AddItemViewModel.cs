using System.Linq;
using System.Windows.Input;
using DynamicData;
using InventorySystem.Models;
using InventorySystem.ViewModels.Singleton;
using ReactiveUI;

namespace InventorySystem.ViewModels;

public class AddItemViewModel
{
    public bool IsEditing { get; }
    public string String => IsEditing ? "Edit Item" : "Add Item";
    public Item Item { get; }

    public ICommand AddItemCommand => ReactiveCommand.Create(AddItem, Item.WhenAnyValue(
        item => item.Name,
        item => item.Quantity,
        (name, qty) => !string.IsNullOrEmpty(name) && qty > 0
    ));

    public AddItemViewModel() : this(false, new Item())
    {
    }

    public AddItemViewModel(bool isEditing, Item item)
    {
        IsEditing = isEditing;
        Item = item;
    }

    private void AddItem()
    {
        if (!IsEditing)
        {
            Item.Id = Context.Instance.Items.Keys.Max() + 1;
        }

        Context.Instance.Items.AddOrUpdate(Item);
    }
}