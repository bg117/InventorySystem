using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using InventorySystem.Common;
using InventorySystem.Models;

namespace InventorySystem.ViewModels
{
    public class TransactionsViewModel : ObservableObject
    {
        public ObservableCollection<Transaction> Transactions { get; } = new ObservableCollection<Transaction>
        {
            new Transaction
            {
                Date = DateTime.Now, Id = Guid.NewGuid(), ItemName = "Vodka 750mL", Quantity = 1,
                Status = TransactionStatus.Processing, TotalPrice = 1100.00m
            },
            new Transaction
            {
                Date = DateTime.Now, Id = Guid.NewGuid(), ItemName = "Loaf of Bread 600g", Quantity = 1,
                Status = TransactionStatus.Processed, TotalPrice = 70.00m
            },
            new Transaction
            {
                Date = DateTime.Now, Id = Guid.NewGuid(), ItemName = "Chuckie 110mL", Quantity = 5,
                Status = TransactionStatus.Pending, TotalPrice = 65.00m
            },
            new Transaction
            {
                Date = DateTime.Now, Id = Guid.NewGuid(), ItemName = "Microsoft 365 Personal 1-Year Subscription",
                Quantity = 1, Status = TransactionStatus.Pending, TotalPrice = 65.00m
            },
        };

        private IEnumerable<Transaction> _selectedTransactions;
        public IEnumerable<Transaction> SelectedTransactions
        {
            get => _selectedTransactions;
            set => SetField(ref _selectedTransactions, value);
        }

        private ICommand _removeSelectedCommand;
        public ICommand RemoveSelectedCommand => _removeSelectedCommand ?? (_removeSelectedCommand = new RelayCommand(
            o =>
            {
                foreach (var transaction in SelectedTransactions)
                {
                    Transactions.Remove(transaction);
                }
            },
            o => SelectedTransactions != null && SelectedTransactions.Any()
        ));
    }
}
