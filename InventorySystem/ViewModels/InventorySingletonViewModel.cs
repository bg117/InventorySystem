using InventorySystem.Common;
using InventorySystem.Models;

namespace InventorySystem.ViewModels;

public sealed class InventorySingletonViewModel : ObservableObject
{
    private ObservableCollectionWithItemNotify<Item> _items = new();
    public ObservableCollectionWithItemNotify<Item> Items
    {
        get => _items;
        set => SetField(ref _items, value);
    }

    private InventorySingletonViewModel() {}

    public static InventorySingletonViewModel Instance { get; set; } = new();
}
