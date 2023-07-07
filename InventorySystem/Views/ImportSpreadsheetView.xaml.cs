using InventorySystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace InventorySystem.Views
{
    /// <summary>
    /// Interaction logic for ImportSpreadsheetView.xaml
    /// </summary>
    public partial class ImportSpreadsheetView
    {
        public ImportSpreadsheetView()
        {
            InitializeComponent();
        }

        private void Browse_OnClick(object sender, RoutedEventArgs e)
        {
            if (DataContext is not ImportSpreadsheetViewModel viewModel)
            {
                return;
            }

            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                DefaultExt = ".xlsx",
                Filter = "Excel Files (*.xlsx)|*.xlsx"
            };

            var result = dialog.ShowDialog();

            if (result == true)
            {
                viewModel.FilePath = dialog.FileName;
            }
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (DataContext is ImportSpreadsheetViewModel viewModel)
            {
                viewModel.Dispose();
            }
        }
    }
}
