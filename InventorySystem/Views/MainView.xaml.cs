using System.Windows;
using InventorySystem.ViewModels;
using Microsoft.Win32;

namespace InventorySystem.Views;

/// <summary>
///     Interaction logic for MainView.xaml
/// </summary>
public partial class MainView
{
    public MainView()
    {
        InitializeComponent();

        // subscribe to ExportFailed event in case no file name
        if (ExportSpreadsheetMenuItem.DataContext is ExportSpreadsheetViewModel exportSpreadsheetViewModel)
        {
            exportSpreadsheetViewModel.ExportFailed += ExportSpreadsheetViewModel_ExportFailed;
            exportSpreadsheetViewModel.ExportCompleted += ExportSpreadsheetViewModel_ExportCompleted;
        }
    }

    private void ExportSpreadsheetViewModel_ExportCompleted()
    {
        MessageBox.Show("Export completed successfully.", "Information", MessageBoxButton.OK,
            MessageBoxImage.Information);
    }

    private void ExportSpreadsheetViewModel_ExportFailed()
    {
        if (ExportSpreadsheetMenuItem.DataContext is not ExportSpreadsheetViewModel exportSpreadsheetViewModel) return;

        var saveFileDialog = new SaveFileDialog
        {
            Filter = "Excel Files (*.xlsx)|*.xlsx",
            DefaultExt = "xlsx",
            AddExtension = true,
            OverwritePrompt = true
        };

        if (saveFileDialog.ShowDialog() != true) return;
        exportSpreadsheetViewModel.FilePath = saveFileDialog.FileName;
    }

    private void FileImportSpreadsheet_OnClick(object sender, RoutedEventArgs e)
    {
        var importSpreadsheetView = new ImportSpreadsheetView();
        importSpreadsheetView.ShowDialog();
    }

    private void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
        Focus();
    }
}