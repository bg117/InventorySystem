using System.Collections.ObjectModel;
using DynamicData;
using InventorySystem.Models;
using ReactiveUI.Fody.Helpers;

namespace InventorySystem.ViewModels.Singleton;

public class Context
{
    public static Context Instance { get; } = new();

    private Context()
    {
        // add some dummy data
        Items.Add(new Item {Name = "Item 1", Description = "Description 1", Quantity = 1, UnitPrice = 30});
        Items.Add(new Item {Name = "Item 2", Description = "Description 2", Quantity = 2, UnitPrice = 20});
        Items.Add(new Item {Name = "Item 3", Description = "Description 3", Quantity = 3, UnitPrice = 10});
        Items.Add(new Item {Name = "Item 4", Description = "Description 4", Quantity = 4, UnitPrice = 40});
    }

    [Reactive] public SourceList<Item> Items { get; } = new();
    [Reactive] public SourceList<Transaction> Transactions { get; set; } = new();
}