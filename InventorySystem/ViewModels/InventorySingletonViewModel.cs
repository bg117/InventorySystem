using System.Collections.ObjectModel;
using InventorySystem.Attributes;
using InventorySystem.Interfaces;
using InventorySystem.Models;
using PostSharp.Patterns.Model;

namespace InventorySystem.ViewModels;

[NotifyPropertyChanged]
public sealed class InventorySingletonViewModel : IChangeTrackable
{
    private InventorySingletonViewModel()
    {
    }

    [AggregateAllChanges]
    [ChangeTracking]
    public ObservableCollection<Item> Items { get; set; } = new();

    public static InventorySingletonViewModel Instance { get; } = new();

    public bool IsChanged { get; set; }
}