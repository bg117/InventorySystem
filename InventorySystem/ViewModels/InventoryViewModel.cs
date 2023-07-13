using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using InventorySystem.Models;
using JetBrains.Annotations;
using PostSharp.Patterns.Model;
using PostSharp.Patterns.Xaml;

namespace InventorySystem.ViewModels;

[NotifyPropertyChanged]
public class InventoryViewModel
{
    public List<Item> SelectedItems { get; set; }

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