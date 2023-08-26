using System;
using System.Windows.Input;
using InventorySystem.Utilities;
using InventorySystem.ViewModels.Singleton;

namespace InventorySystem.ViewModels;

public class TransactionsViewModel : ViewModelBase
{
    public Context Context => Context.Instance;
    
    public ICommand FilterCommand => new RelayCommand(Filter, CanFilter);
    public ICommand AddItemCommand => new RelayCommand(AddItem, CanAddItem);
    public ICommand RemoveItemCommand => new RelayCommand(RemoveItem, CanRemoveItem);
    
    private void Filter()
    {
        throw new NotImplementedException();
    }
    
    private bool CanFilter()
    {
        return true;
    }

    private void AddItem()
    {
        throw new NotImplementedException();
    }
    
    private bool CanAddItem()
    {
        return true;
    }
    
    private void RemoveItem()
    {
        throw new NotImplementedException();
    }
    
    private bool CanRemoveItem()
    {
        return true;
    }
}