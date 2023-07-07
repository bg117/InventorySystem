using InventorySystem.Common;
using InventorySystem.Models;

namespace InventorySystem.ViewModels;

public sealed class TransactionsSingletonViewModel : ObservableObject
{
    public ObservableCollectionWithItemNotify<Transaction> Transactions { get; } = new();

    private TransactionsSingletonViewModel()
    {}

    public static TransactionsSingletonViewModel Instance { get; } = new();
}
