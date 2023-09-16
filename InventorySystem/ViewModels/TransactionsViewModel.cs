using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using InventorySystem.Models;
using JetBrains.Annotations;
using PostSharp.Patterns.Model;
using PostSharp.Patterns.Xaml;

namespace InventorySystem.ViewModels;

[NotifyPropertyChanged]
public class TransactionsViewModel
{
    public TransactionsViewModel()
    {
        var transactions = TransactionsSingletonViewModel.Instance.Transactions;
        var src = CollectionViewSource.GetDefaultView(transactions);

        transactions.CollectionChanged += ItemsOnCollectionChanged;
        src.Filter = SrcFilter;
        FilteredItems = src;
    }

    private DateTime? _dateTimeFilter;
    public DateTime? DateTimeFilter
    {
        get => _dateTimeFilter;
        set
        {
            _dateTimeFilter = value;
            FilteredItems.Refresh();
        }
    }

    private string _idFilter;
    public string IdFilter
    {
        get => _idFilter;
        set
        {
            _idFilter = value;
            FilteredItems.Refresh();
        }
    }

    public ICollectionView FilteredItems { get; }

    public List<Transaction> SelectedTransactions { get; set; }

    [Command]
    [UsedImplicitly]
    public ICommand RemoveSelectedCommand { get; }

    [Command]
    [UsedImplicitly]
    public ICommand CopyIdCommand { get; }

    [UsedImplicitly]
    public bool CanExecuteRemoveSelected => SelectedTransactions?.Count > 0;

    [UsedImplicitly]
    public bool CanExecuteCopyId => SelectedTransactions?.Count > 0;

    [UsedImplicitly]
    public void ExecuteRemoveSelected()
    {
        foreach (var transaction in SelectedTransactions)
            TransactionsSingletonViewModel.Instance.Transactions.Remove(transaction);
    }

    [UsedImplicitly]
    public void ExecuteCopyId()
    {
        Clipboard.SetText(string.Join("\n", SelectedTransactions.Select(t => t.Id.ToString())));
    }

    private bool SrcFilter(object i)
    {
        if (i is not Transaction trans)
            return false;

        var result = true;

        if (DateTimeFilter != null)
            result = trans.Date >= DateTimeFilter;

        if (!string.IsNullOrWhiteSpace(IdFilter))
        {
            var valid = Guid.TryParse(IdFilter, out var guid);
            result = valid && trans.Id == guid;
        }

        return result;
    }

    private void ItemsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.NewItems != null)
            foreach (INotifyPropertyChanged item in e.NewItems)
                item.PropertyChanged += ItemPropertyChanged;

        if (e.OldItems != null)
            foreach (INotifyPropertyChanged item in e.OldItems)
                item.PropertyChanged -= ItemPropertyChanged;

        FilteredItems.SafeRefresh();
    }

    private void ItemPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (sender is not Transaction)
            return;

        FilteredItems.SafeRefresh();
    }
}