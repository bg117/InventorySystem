using System.Collections.ObjectModel;
using InventorySystem.Models;
using ReactiveUI.Fody.Helpers;

namespace InventorySystem.ViewModels.Singleton;

public class Context
{
    public static Context Instance { get; } = new();
    private Context() { }

    [Reactive] public ObservableCollection<Item> Items { get; set; } = new();
    [Reactive] public ObservableCollection<Transaction> Transactions { get; set; } = new();
}