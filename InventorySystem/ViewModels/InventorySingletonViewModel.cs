using InventorySystem.Common;
using InventorySystem.Models;

namespace InventorySystem.ViewModels;

public sealed class InventorySingletonViewModel : ViewModelBase
{
    private ObservableCollectionWithItemNotify<Item> _items = new();

    private InventorySingletonViewModel()
    {
    }

    public ObservableCollectionWithItemNotify<Item> Items
    {
        get => _items;
        set => SetField(ref _items, value);
    }

    public static InventorySingletonViewModel Instance { get; } = new();
}