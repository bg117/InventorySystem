using System.Windows.Input;
using InventorySystem.Models;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Model;
using PostSharp.Patterns.Xaml;

namespace InventorySystem.ViewModels;

[NotifyPropertyChanged]
public class AddItemViewModel
{
    [Required] public string Name { get; set; }

    public string Description { get; set; }

    public int Quantity { get; set; } = 1;

    public decimal Price { get; set; }

    [Command] public ICommand AddItemCommand { get; }

    public bool CanExecuteAddItem => !string.IsNullOrWhiteSpace(Name);

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