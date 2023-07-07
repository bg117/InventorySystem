using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

using InventorySystem.Common;
using InventorySystem.Models;

namespace InventorySystem.ViewModels;

public class TransactionsViewModel : ObservableObject
{
    public TransactionsSingletonViewModel TransactionsSingletonInstance => TransactionsSingletonViewModel.Instance;

    private IEnumerable<Transaction> _selectedTransactions;
    public IEnumerable<Transaction> SelectedTransactions
    {
        get => _selectedTransactions;
        set => SetField(ref _selectedTransactions, value);
    }

    private ICommand _removeSelectedCommand;
    public ICommand RemoveSelectedCommand => _removeSelectedCommand ??= new RelayCommand(
        _ =>
        {
            foreach (var transaction in SelectedTransactions)
            {
                TransactionsSingletonInstance.Transactions.Remove(transaction);
            }
        },
        _ => SelectedTransactions?.Any() == true
    );
}
