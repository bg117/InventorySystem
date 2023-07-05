using System;
using System.Windows.Input;
using InventorySystem.Common;
using InventorySystem.Models;

namespace InventorySystem.ViewModels
{
    public class AddTransactionViewModel : ObservableObject
    {
        public TransactionsSingletonViewModel TransactionsSingletonInstance => TransactionsSingletonViewModel.Instance;
        public InventorySingletonViewModel InventorySingletonInstance => InventorySingletonViewModel.Instance;

        private Item _selectedItem;
        public Item SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetField(ref _selectedItem, value);
                TotalPrice = Quantity * SelectedItem.Price;
                MaximumStock = SelectedItem.Quantity;
            }
        }

        private DateTime? _transactionDate;
        public DateTime TransactionDate
        {
            get => _transactionDate ??= DateTime.Now;
            set => SetField(ref _transactionDate, value);
        }

        private int _quantity = 1;

        public int Quantity
        {
            get => _quantity;
            set
            {
                SetField(ref _quantity, value);
                if (SelectedItem != null)
                {
                    TotalPrice = Quantity * SelectedItem.Price;
                }
            }
        }

        private string _notes = string.Empty;
        public string Notes
        {
            get => _notes;
            set => SetField(ref _notes, value);
        }

        private decimal _totalPrice;
        public decimal TotalPrice
        {
            get => _totalPrice;
            set => SetField(ref _totalPrice, value);
        }

        private int _maximumStock;
        public int MaximumStock
        {
            get => _maximumStock;
            set => SetField(ref _maximumStock, value);
        }

        private ICommand _addTransactionCommand;

        public ICommand AddTransactionCommand => _addTransactionCommand ??= new RelayCommand(o =>
        {
            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                Date = TransactionDate,
                Item = SelectedItem,
                Quantity = Quantity,
                Status = TransactionStatus.Processed,
                TotalPrice = TotalPrice,
                Notes = Notes
            };
            TransactionsSingletonInstance.Transactions.Add(transaction);
            MaximumStock -= Quantity;
            if (Quantity > MaximumStock)
                Quantity = MaximumStock;
            InventorySingletonInstance.Items[InventorySingletonInstance.Items.IndexOf(SelectedItem)].Quantity =
                MaximumStock;
        }, o => SelectedItem != null && MaximumStock > 0 && Quantity <= MaximumStock);
    }
}
