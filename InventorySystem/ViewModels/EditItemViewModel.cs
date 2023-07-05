using System.Windows.Input;
using InventorySystem.Common;
using InventorySystem.Models;

namespace InventorySystem.ViewModels
{
    public class EditItemViewModel : ObservableObject
    {
        public InventorySingletonViewModel InventorySingletonInstance => InventorySingletonViewModel.Instance;

        private Item _item = new();
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

        private int _quantity;
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

        private ICommand _updateItemCommand;

        public ICommand UpdateItemCommand => _updateItemCommand ??= new RelayCommand(o =>
        {
            var idx = InventorySingletonInstance.Items.IndexOf(Item);
            Item.Quantity = Quantity;
            Item.Name = Name;
            Item.Description = Description;
            Item.Price = Price;
            InventorySingletonInstance.Items[idx] = Item;
        }, o => !string.IsNullOrWhiteSpace(Name));
    }
}
