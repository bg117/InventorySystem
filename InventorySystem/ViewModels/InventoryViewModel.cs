using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Util;
using System.Windows.Input;

using InventorySystem.Common;
using InventorySystem.Models;

namespace InventorySystem.ViewModels
{
    public class InventoryViewModel : ObservableObject
    {
        public ObservableCollection<Item> Items { get; } = new ObservableCollection<Item>
        {
            new Item
            {
                Id = Item.NewId(), Name = "Vodka 750mL", Description = "Russian-made alcohol", Price = 1100.00m,
                Quantity = 224
            },
            new Item
            {
                Id = Item.NewId(), Name = "Loaf of Bread 600g", Description = "Gardenia bread", Price = 70.00m,
                Quantity = 420
            },
            new Item
            {
                Id = Item.NewId(), Name = "Chuckie 110mL", Description = "Kids' chocolate drink", Price = 13.00m,
                Quantity = 567
            },
            new Item
            {
                Id = Item.NewId(), Name = "Microsoft 365 Personal 1-Year Subscription",
                Description = "Microsoft Office suite", Price = 3499.00m, Quantity = 100
            },
        };
    }
}
