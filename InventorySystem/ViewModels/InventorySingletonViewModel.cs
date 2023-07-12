using System.Collections.ObjectModel;
using InventorySystem.Models;
using PostSharp.Patterns.Model;

namespace InventorySystem.ViewModels;

[NotifyPropertyChanged]
public sealed class InventorySingletonViewModel
{
    private InventorySingletonViewModel()
    {
    }

    [AggregateAllChanges] public ObservableCollection<Item> Items { get; set; } = new();

    public static InventorySingletonViewModel Instance { get; } = new();
}