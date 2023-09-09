using System;
using System.Collections.Generic;
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
        var src = CollectionViewSource.GetDefaultView(InventorySingletonViewModel.Instance.Items);
        src.Filter = i =>
        {
            if (i is not Item item)
                throw new ArgumentException();

            return item.Name.Contains(Filter, StringComparison.OrdinalIgnoreCase) ||
                   item.Description.Contains(Filter, StringComparison.InvariantCultureIgnoreCase);
        };
        FilteredItems = src;

        InventorySingletonViewModel.Instance.Items.CollectionChanged += (sender, args) =>
        {
            FilteredItems.Refresh();
        };
    }

    public List<Item> SelectedItems { get; set; }

    public string Filter { get; set; } = string.Empty;

    public ICollectionView FilteredItems
    {
        get;
        private set;
    }

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
        foreach (var item in SelectedItems) InventorySingletonViewModel.Instance.Items.Remove(item);
    }

    [UsedImplicitly]
    public void ExecuteCopyId()
    {
        Clipboard.SetText(string.Join("\n", SelectedItems.Select(t => t.Id.ToString())));
    }
}