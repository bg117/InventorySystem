using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using InventorySystem.Models;
using JetBrains.Annotations;
using PostSharp.Patterns.Model;
using PostSharp.Patterns.Xaml;

namespace InventorySystem.ViewModels;

[NotifyPropertyChanged]
public class TransactionsViewModel
{
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
}