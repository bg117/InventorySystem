using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using InventorySystem.Models;
using JetBrains.Annotations;
using PostSharp.Patterns.Model;
using PostSharp.Patterns.Xaml;

namespace InventorySystem.ViewModels;

[NotifyPropertyChanged]
public class InventoryViewModel
{
    public InventoryViewModel()
    {
        var items = InventorySingletonViewModel.Instance.Items;
        var src = CollectionViewSource.GetDefaultView(items);

        items.CollectionChanged += ItemsOnCollectionChanged;
        src.Filter = SrcFilter;
        FilteredItems = src;
    }

    private bool SrcFilter(object i)
    {
        if (i is not Item item)
            return false;

        var isInt = int.TryParse(Filter, out var result);

        return item.Name.Contains(Filter, StringComparison.OrdinalIgnoreCase) ||
               item.Description.Contains(Filter, StringComparison.InvariantCultureIgnoreCase) ||
               (isInt && item.Id == result);
    }

    private void ItemsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.NewItems != null)
            foreach (INotifyPropertyChanged item in e.NewItems)
                item.PropertyChanged += ItemPropertyChanged;

        if (e.OldItems != null)
            foreach (INotifyPropertyChanged item in e.OldItems)
                item.PropertyChanged -= ItemPropertyChanged;

        FilteredItems.SafeRefresh();
    }

    private void ItemPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (sender is not Item)
            return;

        FilteredItems.SafeRefresh();
    }

    public List<Item> SelectedItems { get; set; }

    private string _filter = string.Empty;
    public string Filter
    {
        get => _filter;
        set
        {
            _filter = value;
            FilteredItems.Refresh();
        }
    }

    public ICollectionView FilteredItems { get; }

    public bool HasSelected { [UsedImplicitly] get; set; }

    [Command]
    [UsedImplicitly]
    public ICommand RemoveSelectedCommand { get; }

    [Command]
    [UsedImplicitly]
    public ICommand CopyIdCommand { get; }

    [UsedImplicitly]
    public bool CanExecuteRemoveSelected => SelectedItems?.Count > 0;

    [UsedImplicitly]
    public bool CanExecuteCopyId => SelectedItems?.Count > 0;

    [UsedImplicitly]
    public void ExecuteRemoveSelected()
    {
        foreach (var item in SelectedItems)
            InventorySingletonViewModel.Instance.Items.Remove(item);
    }

    [UsedImplicitly]
    public void ExecuteCopyId()
    {
        Clipboard.SetText(string.Join("\n", SelectedItems.Select(t => t.Id.ToString())));
    }
}