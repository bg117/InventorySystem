using System.Windows;
using InventorySystem.ViewModels;
using Microsoft.Win32;

namespace InventorySystem.Views;

/// <summary>
///     Interaction logic for ImportSpreadsheetView.xaml
/// </summary>
public partial class ImportSpreadsheetView
{
    public ImportSpreadsheetView()
    {
        InitializeComponent();

        if (DataContext is ImportSpreadsheetViewModel viewModel)
            viewModel.SpreadsheetImported += ViewModel_SpreadsheetImported;
    }

    private void Browse_OnClick(object sender, RoutedEventArgs e)
    {
        if (DataContext is not ImportSpreadsheetViewModel viewModel) return;

        var dialog = new OpenFileDialog
        {
            DefaultExt = ".xlsx",
            Filter = "Excel Files (*.xlsx)|*.xlsx"
        };

        var result = dialog.ShowDialog();

        if (result == true) viewModel.FilePath = dialog.FileName;
    }

    private void ViewModel_SpreadsheetImported()
    {
        Close();
    }
}