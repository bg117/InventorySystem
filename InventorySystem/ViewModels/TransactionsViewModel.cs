using System;
using System.Reactive.Linq;
using System.Windows.Input;
using DynamicData;
using DynamicData.Binding;
using InventorySystem.Models;
using InventorySystem.ViewModels.Singleton;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace InventorySystem.ViewModels;

public class TransactionsViewModel : ViewModelBase
{
    public IObservableCollection<Transaction> FilteredTransactions { get; } = new ObservableCollectionExtended<Transaction>();
    
    public TransactionsViewModel()
    {
        Context.Instance.Transactions
            .Connect()
            .AutoRefresh()
            .ObserveOn(RxApp.MainThreadScheduler)
            .Bind(FilteredTransactions)
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