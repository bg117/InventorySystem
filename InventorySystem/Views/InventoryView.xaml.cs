using InventorySystem.Models;
using InventorySystem.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InventorySystem.Views
{
    /// <summary>
    /// Interaction logic for InventoryView.xaml
    /// </summary>
    public partial class InventoryView : UserControl
    {
        public InventoryView()
        {
            InitializeComponent();
        }

        private void AddItem_OnClick(object sender, RoutedEventArgs e)
        {
            new AddItemView().Show();
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is not InventoryViewModel viewModel)
                return;

            viewModel.SelectedItems = InventoryListView.SelectedItems.Cast<Item>().ToList();
            viewModel.HasSelected = viewModel.SelectedItems.Any();
        }

        private void EditSelected_OnClick(object sender, RoutedEventArgs e)
        {
            if (DataContext is not InventoryViewModel viewModel)
                return;

            foreach (var item in viewModel.SelectedItems)
            {
                new EditItemView(item).Show();
            }
        }
    }
}
