using InventorySystem.Common;

namespace InventorySystem.Models
{
    public class Item : ObservableObject
    {
        private int _id;
        public int Id
        {
            get => _id;
            set => SetField(ref _id, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => SetField(ref _name, value);
        }

        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            set => SetField(ref _quantity, value);
        }

        private string _description;
        public string Description
        {
            get => _description;
            set => SetField(ref _description, value);
        }

        private decimal _price;
        public decimal Price
        {
            get => _price;
            set => SetField(ref _price, value);
        }

        private static int _idBase = 1000000000;

        public static int NewId()
        {
            var id = _idBase;
            _idBase += 10;
            return id;
        }
    }
}
