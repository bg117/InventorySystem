using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using InventorySystem.Models;
using PostSharp.Patterns.Model;
using PostSharp.Patterns.Xaml;

namespace InventorySystem.ViewModels;

[NotifyPropertyChanged]
public class TransactionsViewModel
{
    public List<Transaction> SelectedTransactions { get; set; }

    [Command] public ICommand RemoveSelectedCommand { get; }

    [Command] public ICommand CopyIdCommand { get; }

    public bool CanExecuteRemoveSelected => SelectedTransactions?.Count > 0;

    public bool CanExecuteCopyId => SelectedTransactions?.Count > 0;

    public void ExecuteRemoveSelected()
    {
        foreach (var transaction in SelectedTransactions)
            TransactionsSingletonViewModel.Instance.Transactions.Remove(transaction);
    }

    public void ExecuteCopyId()
    {
        Clipboard.SetText(string.Join("\n", SelectedTransactions.Select(t => t.Id.ToString())));
    }
}