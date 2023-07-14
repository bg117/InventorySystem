using System.Windows.Input;
using InventorySystem.Models;
using JetBrains.Annotations;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Model;
using PostSharp.Patterns.Xaml;

namespace InventorySystem.ViewModels;

[NotifyPropertyChanged]
public class AddItemViewModel
{
    [Required]
    public string Name { get; set; }

    public string Description { get; set; }

    public long Quantity { get; set; } = 1;

    public decimal Price { get; set; }

    [Command]
    [UsedImplicitly]
    public ICommand AddItemCommand { get; }

    [UsedImplicitly]
    public bool CanExecuteAddItem => !string.IsNullOrWhiteSpace(Name);

    [UsedImplicitly]
    public void ExecuteAddItem()
    {
        var item = new Item
        {
            Id = Item.NewId(),
            Name = Name,
            Description = Description,
            Quantity = Quantity,
            Price = Price
        };

        InventorySingletonViewModel.Instance.Items.Add(item);
    }
}