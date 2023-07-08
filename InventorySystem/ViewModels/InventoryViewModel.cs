using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

using InventorySystem.Common;
using InventorySystem.Models;

namespace InventorySystem.ViewModels;

public class InventoryViewModel : ViewModelBase
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
    public ICommand RemoveSelectedCommand => _removeSelectedCommand ??= new RelayCommand(RemoveSelected, CanRemoveSelected);

    private ICommand _copyIdCommand;
    public ICommand CopyIdCommand => _copyIdCommand ??= new RelayCommand(CopyId, CanCopyId);

    private void RemoveSelected()
    {
        foreach (var item in SelectedItems)
        {
            InventorySingletonInstance.Items.Remove(item);
        }
    }

    private bool CanRemoveSelected()
    {
        return SelectedItems?.Any() == true;
    }

    private void CopyId()
    {
        Clipboard.SetText(string.Join("\n", SelectedItems.Select(t => t.Id.ToString())));
    }

    private bool CanCopyId()
    {
        return SelectedItems?.Any() == true;
    }
}
