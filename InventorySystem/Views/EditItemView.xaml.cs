using InventorySystem.Models;
using InventorySystem.ViewModels;

namespace InventorySystem.Views;

/// <summary>
/// Interaction logic for AddItemView.xaml
/// </summary>
public partial class EditItemView
{
    public EditItemView()
    {
        InitializeComponent();
    }

    public EditItemView(Item item) : this()
    {
        ((EditItemViewModel)DataContext).Item = item;
    }
}
