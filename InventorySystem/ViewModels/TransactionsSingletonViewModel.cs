using InventorySystem.Common;
using InventorySystem.Models;

namespace InventorySystem.ViewModels;

public sealed class TransactionsSingletonViewModel : ObservableObject
{
    private ObservableCollectionWithItemNotify<Transaction> _transactions = new();
    public ObservableCollectionWithItemNotify<Transaction> Transactions
    {
        get => _transactions;
        set => SetField(ref _transactions, value);
    }

    private TransactionsSingletonViewModel()
    {}

    public static TransactionsSingletonViewModel Instance { get; } = new();
}
