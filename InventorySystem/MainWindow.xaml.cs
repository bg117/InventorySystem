using System.Windows;
using InventorySystem.Views;
using MahApps.Metro.Controls;

namespace InventorySystem;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : MetroWindow
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void FileImportSpreadsheet_OnClick(object sender, RoutedEventArgs e)
    {
        var importSpreadsheetView = new ImportSpreadsheetView();
        importSpreadsheetView.ShowDialog();
    }
}