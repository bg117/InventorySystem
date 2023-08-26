using System;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace InventorySystem.Models;

public class Item : ReactiveObject
{
    [Reactive] public int Id { get; set; }
    [Reactive] public string Name { get; set; }
    [Reactive] public string Description { get; set; }
    [Reactive] public int Quantity { get; set; }
    [Reactive] public decimal UnitPrice { get; set; }

    public Item()
    {
        Name = string.Empty;
        Description = string.Empty;
    }
}