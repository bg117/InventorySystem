using System;
using System.Windows.Input;
using InventorySystem.Common;
using InventorySystem.Models;

namespace InventorySystem.ViewModels;

public class AddTransactionViewModel : ObservableObject
{
    public TransactionsSingletonViewModel TransactionsSingletonInstance => TransactionsSingletonViewModel.Instance;
    public InventorySingletonViewModel InventorySingletonInstance => InventorySingletonViewModel.Instance;

    private Item _selectedItem;
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

    private DateTime? _transactionDate;
    public DateTime TransactionDate
    {
        get => _transactionDate ?? DateTime.Now;
        set => SetField(ref _transactionDate, value);
    }

    private int _stockOut;

    public int StockOut
    {
        get => _stockOut;
        set
        {
            SetField(ref _stockOut, value);
            TotalPrice = ComputeTotalPrice();
        }
    }

    private int _stockIn;

    public int StockIn
    {
        get => _stockIn;
        set
        {
            SetField(ref _stockIn, value);
            TotalPrice = ComputeTotalPrice();
        }
    }

    private string _notes = string.Empty;
    public string Notes
    {
        get => _notes;
        set => SetField(ref _notes, value);
    }

    private decimal _totalPrice;
    public decimal TotalPrice
    {
        get => _totalPrice;
        set => SetField(ref _totalPrice, value);
    }

    private int _maximumStock;
    public int MaximumStock
    {
        get => _maximumStock;
        set => SetField(ref _maximumStock, value);
    }

    private ICommand _addTransactionCommand;

    public ICommand AddTransactionCommand => _addTransactionCommand ??= new RelayCommand(AddTransaction, CanAddTransaction);

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

        TransactionsSingletonInstance.Transactions.Insert(0, transaction);

        MaximumStock -= StockOut - StockIn;

        var itemIndex = InventorySingletonInstance.Items.IndexOf(SelectedItem);
        InventorySingletonInstance.Items[itemIndex].Quantity = MaximumStock;
    }

    private decimal ComputeTotalPrice()
    {
        return SelectedItem == null ? 0 : (StockOut - StockIn) * SelectedItem.Price;
    }
}
