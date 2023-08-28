using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using InventorySystem.ViewModels;

namespace InventorySystem.Views;

public partial class AddItemView : Window
{
    public AddItemView(AddItemViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}