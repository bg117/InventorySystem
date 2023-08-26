using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using InventorySystem.ViewModels;

namespace InventorySystem.Views;

public partial class InventoryView : UserControl
{
    public InventoryView()
    {
        InitializeComponent();
        DataContext = new InventoryViewModel();
    }
}