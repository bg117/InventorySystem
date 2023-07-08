using InventorySystem.Common;
using InventorySystem.Models;

namespace InventorySystem.ViewModels;

public sealed class TransactionsSingletonViewModel : ViewModelBase
{
    private ObservableCollectionWithItemNotify<Transaction> _transactions = new();

    private TransactionsSingletonViewModel()
    {
    }

    public ObservableCollectionWithItemNotify<Transaction> Transactions
    {
        get => _transactions;
        set => SetField(ref _transactions, value);
    }

    public static TransactionsSingletonViewModel Instance { get; } = new();
}