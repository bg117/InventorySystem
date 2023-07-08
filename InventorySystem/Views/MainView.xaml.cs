using System.Windows;
using System.Windows.Controls;

namespace InventorySystem.Views;

/// <summary>
///     Interaction logic for MainView.xaml
/// </summary>
public partial class MainView
{
    public MainView()
    {
        InitializeComponent();
    }

    private void FileImportSpreadsheet_OnClick(object sender, RoutedEventArgs e)
    {
        var importSpreadsheetView = new ImportSpreadsheetView();
        importSpreadsheetView.ShowDialog();
    }
}