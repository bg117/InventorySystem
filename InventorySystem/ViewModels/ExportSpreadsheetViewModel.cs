using System;
using System.IO;
using System.Linq;
using System.Windows.Input;
using JetBrains.Annotations;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using PostSharp.Patterns.Model;
using PostSharp.Patterns.Xaml;

namespace InventorySystem.ViewModels;

[NotifyPropertyChanged]
public class ExportSpreadsheetViewModel
{
    public string FilePath { get; set; }

    [Command]
    [UsedImplicitly]
    public ICommand ExportCommand { get; }

    public event Action ExportFailed;
    public event Action ExportCompleted;

    [UsedImplicitly]
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
        var items = InventorySingletonViewModel.Instance.Items.Select(item => new object[]
            { item.Id, item.Name, item.Description, item.Quantity, item.Price }).ToArray();

        if (inventoryTable == null)
        {
            var tableRange = inventorySheet.Cells["A1:E2"];
            var table = inventorySheet.Tables.Add(tableRange, instance.InventoryTableName);

            table.ShowHeader = true;
            table.TableStyle = TableStyles.Medium2;
            // add headers
            tableRange.Value = new object[,]
            {
                { "ID", "Name", "Description", "Quantity", "Price" }
            };

            inventoryTable = table;
            instance.InventoryTableName = table.Name;
        }
        else
        {
            inventoryTable.AddRow();
            var tableRange = inventorySheet.Cells[inventoryTable.Address.Start.Row + 1,
                inventoryTable.Address.Start.Column, inventoryTable.Address.End.Row - 1,
                inventoryTable.Address.End.Column];
            tableRange.Delete(eShiftTypeDelete.Up);
        }

        for (var i = 0; i < items.Length - 1; i++)
            inventoryTable.AddRow();
        inventoryTable.Range.Offset(1, 0).LoadFromArrays(items);

        InventorySingletonViewModel.Instance.IsChanged = false;
    }

    private static void ExportTransactions(ExcelWorksheets worksheets, SpreadsheetSingletonViewModel instance)
    {
        instance.TransactionSheetName ??= "Transactions";
        instance.TransactionTableName ??= "Transactions";
        var transactionSheet =
            worksheets[instance.TransactionSheetName] ?? worksheets.Add(instance.TransactionSheetName);
        var transactionTable = transactionSheet.Tables[instance.TransactionTableName];
        var transactions = TransactionsSingletonViewModel.Instance.Transactions.Select(transaction => new object[]
        {
            transaction.Id, transaction.Date, transaction.Item.Id, transaction.StockIn, transaction.StockOut,
            transaction.Status, transaction.TotalPrice, transaction.Notes
        }).ToArray();

        if (transactionTable == null)
        {
            var tableRange = transactionSheet.Cells["A1:H2"];
            var table = transactionSheet.Tables.Add(tableRange,
                instance.TransactionTableName);

            table.ShowHeader = true;
            table.TableStyle = TableStyles.Medium2;

            // add headers
            tableRange.Value = new object[,]
            {
                {
                    "Transaction ID", "Transaction Date", "Item ID", "Stock In", "Stock Out", "Status", "Total Price",
                    "Notes"
                }
            };

            transactionTable = table;
            instance.TransactionTableName = table.Name;
        }
        else
        {
            transactionTable.AddRow();
            var tableRange = transactionSheet.Cells[transactionTable.Address.Start.Row + 1,
                transactionTable.Address.Start.Column, transactionTable.Address.End.Row - 1,
                transactionTable.Address.End.Column];
            tableRange.Delete(eShiftTypeDelete.Up);
        }

        for (var i = 0; i < transactions.Length - 1; i++)
            transactionTable.AddRow();

        transactionTable.Range.Offset(1, 0).LoadFromArrays(transactions);
        // format 2nd column as date
        transactionSheet.Cells[transactionTable.Address.Start.Row + 1, 2,
            transactionTable.Address.End.Row, 2].Style.Numberformat.Format = "m/d/yy h:mm";

        TransactionsSingletonViewModel.Instance.IsChanged = false;
    }
}