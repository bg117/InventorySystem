using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

using InventorySystem.Common;
using InventorySystem.Models;

namespace InventorySystem.ViewModels;

public class InventoryViewModel : ObservableObject
{
    public InventorySingletonViewModel InventorySingletonInstance => InventorySingletonViewModel.Instance;

    private IEnumerable<Item> _selectedItems;
    public IEnumerable<Item> SelectedItems
    {
        get => _selectedItems;
        set => SetField(ref _selectedItems, value);
    }

    private bool _hasSelected;
    public bool HasSelected
    {
        get => _hasSelected;
        set => SetField(ref _hasSelected, value);
    }

    private ICommand _removeSelectedCommand;
    public ICommand RemoveSelectedCommand => _removeSelectedCommand ??= new RelayCommand(
        _ =>
        {
            foreach (var transaction in SelectedItems)
            {
                InventorySingletonInstance.Items.Remove(transaction);
            }
        },
        _ => SelectedItems?.Any() == true
    );

    private ICommand _copyIdCommand;
    public ICommand CopyIdCommand => _copyIdCommand ??= new RelayCommand(_ =>
    {
        Clipboard.SetText(string.Join("\n", SelectedItems.Select(t => t.Id.ToString())));
    });
}
