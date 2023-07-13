using System;
using InventorySystem.Attributes;
using InventorySystem.Interfaces;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Model;

namespace InventorySystem.Models;

public enum TransactionStatus
{
    Processed,
    Processing,
    Pending,
    Failed
}

[NotifyPropertyChanged]
public class Transaction
{
    public Guid Id { get; set; }

    public DateTime Date { get; set; }

    [Required]
    public Item Item { get; set; }
    
    public int StockIn { get; set; }
    
    public int StockOut { get; set; }
    
    public TransactionStatus Status { get; set; }
    
    public decimal TotalPrice { get; set; }
    
    public string Notes { get; set; }
}