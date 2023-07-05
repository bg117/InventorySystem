using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

using InventorySystem.Common;
using InventorySystem.Models;

namespace InventorySystem.ViewModels
{
    public class TransactionsViewModel : ObservableObject
    {
        public TransactionsSingletonViewModel TransactionsSingletonInstance => TransactionsSingletonViewModel.Instance;

        private ObservableCollectionWithItemNotify<Transaction> _filteredTransactions;

        public ObservableCollectionWithItemNotify<Transaction> FilteredTransactions
        {
            get => _filteredTransactions ??= TransactionsSingletonInstance.Transactions;
            set => SetField(ref _filteredTransactions, value);
        }

        private IEnumerable<Transaction> _selectedTransactions;
        public IEnumerable<Transaction> SelectedTransactions
        {
            get => _selectedTransactions;
            set => SetField(ref _selectedTransactions, value);
        }

        private ICommand _removeSelectedCommand;
        public ICommand RemoveSelectedCommand => _removeSelectedCommand ??= new RelayCommand(
            o =>
            {
                foreach (var transaction in SelectedTransactions)
                {
                    TransactionsSingletonInstance.Transactions.Remove(transaction);
                    FilteredTransactions.Remove(transaction);
                }
            },
            o => SelectedTransactions != null && SelectedTransactions.Any()
        );

        private string _idFilter;

        public string IdFilter
        {
            get => _idFilter;
            set => SetField(ref _idFilter, value);
        }

        private string _nameFilter;

        public string NameFilter
        {
            get => _nameFilter;
            set => SetField(ref _nameFilter, value);
        }

        private ICommand _searchIdCommand;
        public ICommand SearchIdCommand => _searchIdCommand ??= new RelayCommand(o =>
        {
            var res = Guid.TryParse(IdFilter, out var guid);
            var filtered = !res
                ? TransactionsSingletonInstance.Transactions
                : TransactionsSingletonInstance.Transactions.Where(t => t.Id == guid);
            FilteredTransactions = new ObservableCollectionWithItemNotify<Transaction>(filtered);
        });

        private ICommand _filterNameCommand;
        public ICommand FilterNameCommand => _filterNameCommand ??= new RelayCommand(o =>
        {
            var filtered = string.IsNullOrWhiteSpace(NameFilter)
                ? TransactionsSingletonInstance.Transactions
                : TransactionsSingletonInstance.Transactions.Where(t => t.Item.Name.IndexOf(NameFilter, StringComparison.OrdinalIgnoreCase) >= 0);
            FilteredTransactions = new ObservableCollectionWithItemNotify<Transaction>(filtered);
        });

        private ICommand _copyIdCommand;
        public ICommand CopyIdCommand => _copyIdCommand ??= new RelayCommand(o =>
        {
            Clipboard.SetText(string.Join("\n", SelectedTransactions.Select(t => t.Id.ToString())));
        });
    }
}
