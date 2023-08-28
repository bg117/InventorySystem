using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using InventorySystem.ViewModels;

namespace InventorySystem.Views;

public partial class InventoryView : UserControl
{
    public InventoryView()
    {
        InitializeComponent();
        DataContext = new InventoryViewModel();
    }

    private void FilterButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (DataContext is not InventoryViewModel viewModel) return;

        var filterWindow = new InventoryFilterView
        {
            DataContext = viewModel.FilterViewModel
        };

        filterWindow.Show();
    }

    private void AddButton_OnClick(object? sender, RoutedEventArgs e)
    {
        var addItemViewModel = new AddItemViewModel();
        var addItemWindow = new AddItemView(addItemViewModel);
        addItemWindow.Show();
    }
}