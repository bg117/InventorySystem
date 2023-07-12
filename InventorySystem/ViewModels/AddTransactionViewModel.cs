using System;
using System.Diagnostics;
using System.Windows.Input;
using InventorySystem.Models;
using PostSharp.Patterns.Model;
using PostSharp.Patterns.Xaml;

namespace InventorySystem.ViewModels;

[NotifyPropertyChanged]
public class AddTransactionViewModel
{
    public Item SelectedItem { get; set; }

    public DateTime TransactionDate { get; set; } = DateTime.Now;

    public int StockIn { get; set; }

    public int StockOut { get; set; }

    public TransactionStatus? Status { get; set; }

    public string Notes { get; set; }

    public decimal TotalPrice => ComputeTotalPrice();

    public int MaximumStock => SelectedItem?.Quantity ?? 0;

    [Command] public ICommand AddTransactionCommand { get; }

    public bool CanExecuteAddTransaction =>
        SelectedItem != null && MaximumStock + StockIn > 0 && StockOut <= MaximumStock + StockIn && Status != null;

    public void ExecuteAddTransaction()
    {
        var transaction = new Transaction
        {
            Id = Guid.NewGuid(),
            Date = TransactionDate,
            Item = SelectedItem,
            StockOut = StockOut,
            StockIn = StockIn,
            Status = Status.Value!,
            TotalPrice = TotalPrice,
            Notes = Notes
        };

        TransactionsSingletonViewModel.Instance.Transactions.Insert(0, transaction);

        var itemIndex = InventorySingletonViewModel.Instance.Items.IndexOf(SelectedItem);
        InventorySingletonViewModel.Instance.Items[itemIndex].Quantity -= StockOut - StockIn;
    }

    private decimal ComputeTotalPrice()
    {
        return SelectedItem == null ? 0 : (StockOut - StockIn) * SelectedItem.Price;
    }
}