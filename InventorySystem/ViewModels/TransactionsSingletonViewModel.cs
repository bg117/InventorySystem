using System.Collections.ObjectModel;
using InventorySystem.Attributes;
using InventorySystem.Interfaces;
using InventorySystem.Models;
using PostSharp.Patterns.Model;

namespace InventorySystem.ViewModels;

[NotifyPropertyChanged]
public sealed class TransactionsSingletonViewModel : IChangeTrackable
{
    private TransactionsSingletonViewModel()
    {
    }

    [AggregateAllChanges]
    [ChangeTracking]
    public ObservableCollection<Transaction> Transactions { get; set; } = new();

    public static TransactionsSingletonViewModel Instance { get; } = new();

    public bool IsChanged { get; set; }
}