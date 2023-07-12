using System;
using System.IO;
using System.Windows.Input;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using PostSharp.Patterns.Model;
using PostSharp.Patterns.Xaml;

namespace InventorySystem.ViewModels;

[NotifyPropertyChanged]
public class ExportSpreadsheetViewModel
{
    public string FilePath { get; set; }

    [Command] public ICommand ExportCommand { get; }

    public event Action ExportFailed;
    public event Action ExportCompleted;

    public void ExecuteExport()
    {
        if (string.IsNullOrWhiteSpace(FilePath)) ExportFailed?.Invoke();
        if (string.IsNullOrWhiteSpace(FilePath)) return;

        using var spreadsheet = new ExcelPackage(new FileInfo(FilePath));
        var worksheets = spreadsheet.Workbook.Worksheets;

        var instance = SpreadsheetSingletonViewModel.Instance;
        instance.FilePath = FilePath;

        ExportInventory(instance, worksheets);
        ExportTransactions(worksheets, instance);

        spreadsheet.Save();

        ExportCompleted?.Invoke();
    }

    private static void ExportInventory(SpreadsheetSingletonViewModel instance, ExcelWorksheets worksheets)
    {
        instance.InventorySheetName ??= "Inventory";
        instance.InventoryTableName ??= "Inventory";

        var inventorySheet = worksheets[instance.InventorySheetName] ?? worksheets.Add(instance.InventorySheetName);
        var inventoryTable = inventorySheet.Tables[instance.InventoryTableName];

        if (inventoryTable == null)
        {
            var table = inventorySheet.Tables.Add(inventorySheet.Cells[1, 1, 2, 5], instance.InventoryTableName);
            var row = table.Range.Start.Row;

            table.ShowHeader = true;
            table.TableStyle = TableStyles.Medium2;
            // add headers
            inventorySheet.Cells[row, 1].Value = "Id";
            inventorySheet.Cells[row, 2].Value = "Name";
            inventorySheet.Cells[row, 3].Value = "Description";
            inventorySheet.Cells[row, 4].Value = "Quantity";
            inventorySheet.Cells[row, 5].Value = "Price";

            inventoryTable = table;
            instance.InventoryTableName = table.Name;
        }

        foreach (var item in InventorySingletonViewModel.Instance.Items)
        {
            var row = inventoryTable.AddRow().Start.Row;

            inventorySheet.Cells[row, 1].Value = item.Id;
            inventorySheet.Cells[row, 2].Value = item.Name;
            inventorySheet.Cells[row, 3].Value = item.Description;
            inventorySheet.Cells[row, 4].Value = item.Quantity;
            inventorySheet.Cells[row, 5].Value = item.Price;
        }
    }

    private static void ExportTransactions(ExcelWorksheets worksheets, SpreadsheetSingletonViewModel instance)
    {
        instance.TransactionSheetName ??= "Transactions";
        instance.TransactionTableName ??= "Transactions";

        var transactionSheet =
            worksheets[instance.TransactionSheetName] ?? worksheets.Add(instance.TransactionSheetName);
        var transactionTable = transactionSheet.Tables[instance.TransactionTableName];

        if (transactionTable == null)
        {
            var table = transactionSheet.Tables.Add(transactionSheet.Cells[1, 1, 2, 8],
                instance.TransactionTableName);
            var row = table.Range.Start.Row;

            table.ShowHeader = true;
            table.TableStyle = TableStyles.Medium2;

            // add headers
            transactionSheet.Cells[row, 1].Value = "Id";
            transactionSheet.Cells[row, 2].Value = "Date";
            transactionSheet.Cells[row, 3].Value = "Item Id";
            transactionSheet.Cells[row, 4].Value = "Stock In";
            transactionSheet.Cells[row, 5].Value = "Stock Out";
            transactionSheet.Cells[row, 6].Value = "Status";
            transactionSheet.Cells[row, 7].Value = "Total Price";
            transactionSheet.Cells[row, 8].Value = "Notes";

            transactionTable = table;
            instance.TransactionTableName = table.Name;
        }

        foreach (var transaction in TransactionsSingletonViewModel.Instance.Transactions)
        {
            var row = transactionTable.AddRow().Start.Row;

            transactionSheet.Cells[row, 1].Value = transaction.Id;
            transactionSheet.Cells[row, 2].Value = transaction.Date;
            transactionSheet.Cells[row, 3].Value = transaction.Item.Id;
            transactionSheet.Cells[row, 4].Value = transaction.StockIn;
            transactionSheet.Cells[row, 5].Value = transaction.StockOut;
            transactionSheet.Cells[row, 6].Value = transaction.Status;
            transactionSheet.Cells[row, 7].Value = transaction.TotalPrice;
            transactionSheet.Cells[row, 8].Value = transaction.Notes;
        }
    }
}