using PostSharp.Patterns.Model;

namespace InventorySystem.ViewModels;

[NotifyPropertyChanged]
public class ErrorViewModel
{
    public string Error { get; set; }

    public bool HasError { get; set; }
}