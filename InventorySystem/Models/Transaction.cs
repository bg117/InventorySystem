using System;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace InventorySystem.Models;

public class Transaction : ReactiveObject
{
    [Reactive] public Guid Id { get; set; }
    [Reactive] public DateTime Date { get; set; }
    [Reactive] public Item Item { get; set; }
    [Reactive] public int In { get; set; }
    [Reactive] public int Out { get; set; }
    [Reactive] public decimal Total { get; set; }
    [Reactive] public string? Remarks { get; set; }
}