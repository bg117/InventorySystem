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
