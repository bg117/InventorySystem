using System;
using System.Windows.Input;
using InventorySystem.Common;
using InventorySystem.Models;

namespace InventorySystem.ViewModels;

public class AddTransactionViewModel : ViewModelBase
{
    private ICommand _addTransactionCommand;

    private int _maximumStock;

    private string _notes = string.Empty;
    private Item _selectedItem;

    private int _stockIn;

    private int _stockOut;

    private decimal _totalPrice;

    private DateTime? _transactionDate;

    public Item SelectedItem
    {
        get => _selectedItem;
        set
        {
            SetField(ref _selectedItem, value);
            MaximumStock = SelectedItem.Quantity;
            TotalPrice = ComputeTotalPrice();
        }
    }

    public DateTime TransactionDate
    {
        get => _transactionDate ?? DateTime.Now;
        set => SetField(ref _transactionDate, value);
    }

    public int StockIn
    {
        get => _stockIn;
        set
        {
            SetField(ref _stockIn, value);
            TotalPrice = ComputeTotalPrice();
        }
    }

    public int StockOut
    {
        get => _stockOut;
        set
        {
            SetField(ref _stockOut, value);
            TotalPrice = ComputeTotalPrice();
        }
    }

    public string Notes
    {
        get => _notes;
        set => SetField(ref _notes, value);
    }

    public decimal TotalPrice
    {
        get => _totalPrice;
        private set => SetField(ref _totalPrice, value);
    }

    public int MaximumStock
    {
        get => _maximumStock;
        set => SetField(ref _maximumStock, value);
    }

    public ICommand AddTransactionCommand =>
        _addTransactionCommand ??= new RelayCommand(AddTransaction, CanAddTransaction);

    private bool CanAddTransaction()
    {
        return SelectedItem != null && MaximumStock + StockIn > 0 && StockOut <= MaximumStock + StockIn;
    }

    private void AddTransaction()
    {
        var transaction = new Transaction
        {
            Id = Guid.NewGuid(),
            Date = TransactionDate,
            Item = SelectedItem,
            StockOut = StockOut,
            StockIn = StockIn,
            Status = TransactionStatus.Processed,
            TotalPrice = TotalPrice,
            Notes = Notes
        };

        TransactionsSingletonViewModel.Instance.Transactions.Insert(0, transaction);

        MaximumStock -= StockOut - StockIn;

        var itemIndex = InventorySingletonViewModel.Instance.Items.IndexOf(SelectedItem);
        InventorySingletonViewModel.Instance.Items[itemIndex].Quantity = MaximumStock;
    }

    private decimal ComputeTotalPrice()
    {
        return SelectedItem == null ? 0 : (StockOut - StockIn) * SelectedItem.Price;
    }
}