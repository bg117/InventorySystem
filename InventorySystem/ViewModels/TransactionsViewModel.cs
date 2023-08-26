using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Windows.Input;
using DynamicData;
using InventorySystem.Models;
using InventorySystem.ViewModels.Singleton;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace InventorySystem.ViewModels;

public class TransactionsViewModel : ViewModelBase
{
    private readonly ReadOnlyObservableCollection<Transaction> _filteredTransactions;
    public ReadOnlyObservableCollection<Transaction> FilteredTransactions => _filteredTransactions;
    
    public TransactionsViewModel()
    {
        Context.Instance.Transactions
            .Connect()
            .ObserveOn(RxApp.MainThreadScheduler)
            .Bind(out _filteredTransactions)
            .Subscribe();
    }

    [Reactive] public string SearchQuery { get; set; } = string.Empty;
    
    public ICommand FilterCommand => ReactiveCommand.Create(Filter);
    public ICommand AddItemCommand => ReactiveCommand.Create(AddItem);
    public ICommand RemoveItemCommand => ReactiveCommand.Create(RemoveItem);

    private void Filter()
    {
        throw new NotImplementedException();
    }

    private void AddItem()
    {
        throw new NotImplementedException();
    }

    private void RemoveItem()
    {
        throw new NotImplementedException();
    }
}