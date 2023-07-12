using System.Collections.ObjectModel;
using InventorySystem.Models;
using PostSharp.Patterns.Model;

namespace InventorySystem.ViewModels;

[NotifyPropertyChanged]
public sealed class TransactionsSingletonViewModel
{
    private TransactionsSingletonViewModel()
    {
    }

    [AggregateAllChanges] public ObservableCollection<Transaction> Transactions { get; set; } = new();

    public static TransactionsSingletonViewModel Instance { get; } = new();
}