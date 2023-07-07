using InventorySystem.Common;
using InventorySystem.Models;

namespace InventorySystem.ViewModels;

public sealed class InventorySingletonViewModel : ObservableObject
{
    public ObservableCollectionWithItemNotify<Item> Items { get; } = new();

    private InventorySingletonViewModel() {}

    public static InventorySingletonViewModel Instance { get; } = new();
}
