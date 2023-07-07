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

public class Transaction : ObservableObject
{
    private decimal _totalPrice;
    private Guid _id;
    private DateTime _date;
    private Item _item;
    private string _notes;
    private int _quantity;
    private TransactionStatus _status;

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
    public int Quantity
    {
        get => _quantity;
        set => SetField(ref _quantity, value);
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
