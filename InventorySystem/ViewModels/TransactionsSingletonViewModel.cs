using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using InventorySystem.Common;
using InventorySystem.Models;

namespace InventorySystem.ViewModels
{
    public class TransactionsSingletonViewModel : ObservableObject
    {
        public ObservableCollectionWithItemNotify<Transaction> Transactions { get; } = new ObservableCollectionWithItemNotify<Transaction>();

        private TransactionsSingletonViewModel()
        {}

        public static TransactionsSingletonViewModel Instance { get; } = new TransactionsSingletonViewModel();
    }
}
