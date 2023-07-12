using System.Linq;
using System.Windows;
using System.Windows.Controls;
using InventorySystem.Models;
using InventorySystem.ViewModels;

namespace InventorySystem.Views;

/// <summary>
///     Interaction logic for InventoryView.xaml
/// </summary>
public partial class InventoryView
{
    public InventoryView()
    {
        InitializeComponent();
    }

    private void AddItem_OnClick(object sender, RoutedEventArgs e)
    {
        var addItemView = new AddItemView();
        addItemView.ShowDialog();
    }

    private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (DataContext is not InventoryViewModel viewModel)
            return;

        viewModel.SelectedItems = InventoryDataGrid.SelectedItems.Cast<Item>().ToList();
        viewModel.HasSelected = viewModel.SelectedItems.Any();
    }
}