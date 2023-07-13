using System.Windows;
using OfficeOpenXml;
using PostSharp.Patterns.Diagnostics;
using PostSharp.Patterns.Diagnostics.Backends.Console;

namespace InventorySystem;

/// <summary>
///     Interaction logic for App.xaml
/// </summary>
public partial class App
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        LoggingServices.DefaultBackend = new ConsoleLoggingBackend();
    }
}