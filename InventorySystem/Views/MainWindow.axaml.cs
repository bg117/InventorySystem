using Avalonia.Controls;
using InventorySystem.ViewModels;

namespace InventorySystem.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowViewModel();
    }
}