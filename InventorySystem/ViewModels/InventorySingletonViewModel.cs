using InventorySystem.Common;
using InventorySystem.Models;

namespace InventorySystem.ViewModels
{
    public class InventorySingletonViewModel : ObservableObject
    {
        public ObservableCollectionWithItemNotify<Item> Items { get; } = new ObservableCollectionWithItemNotify<Item>();

        private InventorySingletonViewModel() {}

        public static InventorySingletonViewModel Instance { get; } = new InventorySingletonViewModel();
    }
}
