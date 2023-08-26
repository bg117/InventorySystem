using System;
using ReactiveUI.Fody.Helpers;

namespace InventorySystem.Models;

public class Item
{
    [Reactive] public int Id { get; }
    [Reactive] public string Name { get; set; }
    [Reactive] public string? Description { get; set; }
    [Reactive] public int Quantity { get; set; }
    [Reactive] public decimal UnitPrice { get; set; }
    
    private static int _idCounter = 1;
    
    public Item()
    {
        Id = _idCounter++;
    }
}