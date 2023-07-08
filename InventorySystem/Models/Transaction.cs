using System;
using InventorySystem.Common;

namespace InventorySystem.Models;

public enum TransactionStatus
{
    Processed,
    Processing,
    Pending,
    Failed
}

public class Transaction : ViewModelBase
{
    private DateTime _date;
    private Guid _id;
    private Item _item;
    private string _notes;
    private TransactionStatus _status;
    private int _stockIn;
    private int _stockOut;
    private decimal _totalPrice;

    public Guid Id
    {
        get => _id;
        set => SetField(ref _id, value);
    }

    public DateTime Date
    {
        get => _date;
        set => SetField(ref _date, value);
    }

    public Item Item
    {
        get => _item;
        set => SetField(ref _item, value);
    }

    public string Notes
    {
        get => _notes;
        set => SetField(ref _notes, value);
    }

    public int StockOut
    {
        get => _stockOut;
        set => SetField(ref _stockOut, value);
    }

    public int StockIn
    {
        get => _stockIn;
        set => SetField(ref _stockIn, value);
    }

    public TransactionStatus Status
    {
        get => _status;
        set => SetField(ref _status, value);
    }

    public decimal TotalPrice
    {
        get => _totalPrice;
        set => SetField(ref _totalPrice, value);
    }
}