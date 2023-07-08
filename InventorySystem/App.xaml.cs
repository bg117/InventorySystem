using OfficeOpenXml;
using System.Windows;

namespace InventorySystem;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
    }
}
