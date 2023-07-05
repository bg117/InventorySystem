﻿using System;
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
using InventorySystem.Models;
using InventorySystem.ViewModels;

namespace InventorySystem.Views
{
    /// <summary>
    /// Interaction logic for TransactionsView.xaml
    /// </summary>
    public partial class TransactionsView : UserControl
    {
        public TransactionsView()
        {
            InitializeComponent();
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is not TransactionsViewModel viewModel)
                return;

            viewModel.SelectedTransactions = TransactionsListView.SelectedItems.Cast<Transaction>().ToList();
        }

        private void AddTransaction_OnClick(object sender, RoutedEventArgs e)
        {
            new AddTransactionView().Show();
        }
    }
}
