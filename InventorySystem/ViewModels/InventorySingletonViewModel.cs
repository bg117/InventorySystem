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
    public class InventorySingletonViewModel : ObservableObject
    {
        public ObservableCollectionWithItemNotify<Item> Items { get; } = new ObservableCollectionWithItemNotify<Item>();

        private InventorySingletonViewModel() {}

        public static InventorySingletonViewModel Instance { get; } = new InventorySingletonViewModel();
    }
}
