using System.Windows;

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

    private void FileExportSpreadsheet_OnClick(object sender, RoutedEventArgs e)
    {
        throw new System.NotImplementedException();
    }
}