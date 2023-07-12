using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using InventorySystem.Models;
using PostSharp.Patterns.Model;
using PostSharp.Patterns.Xaml;

namespace InventorySystem.ViewModels;

[NotifyPropertyChanged]
public class InventoryViewModel
{
    public List<Item> SelectedItems { get; set; }

    public bool HasSelected { get; set; }

    [Command] public ICommand RemoveSelectedCommand { get; }

    [Command] public ICommand CopyIdCommand { get; }

    public bool CanExecuteRemoveSelected => SelectedItems?.Count > 0;

    public bool CanExecuteCopyId => SelectedItems?.Count > 0;

    public void ExecuteRemoveSelected()
    {
        foreach (var item in SelectedItems) InventorySingletonViewModel.Instance.Items.Remove(item);
    }

    public void ExecuteCopyId()
    {
        Clipboard.SetText(string.Join("\n", SelectedItems.Select(t => t.Id.ToString())));
    }
}