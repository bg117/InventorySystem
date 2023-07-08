using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using InventorySystem.Common;
using InventorySystem.Models;

namespace InventorySystem.ViewModels;

public class TransactionsViewModel : ViewModelBase
{
    private ICommand _removeSelectedCommand;

    private IEnumerable<Transaction> _selectedTransactions;

    public IEnumerable<Transaction> SelectedTransactions
    {
        get => _selectedTransactions;
        set => SetField(ref _selectedTransactions, value);
    }

    public ICommand RemoveSelectedCommand => _removeSelectedCommand ??= new RelayCommand(CopyId, CanCopyId);

    private void CopyId()
    {
        Clipboard.SetText(string.Join("\n", SelectedTransactions.Select(t => t.Id.ToString())));
    }

    private bool CanCopyId()
    {
        return SelectedTransactions?.Any() == true;
    }
}