using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Input;
using InventorySystem.Common;
using InventorySystem.Models;
using OfficeOpenXml;
using OfficeOpenXml.Table;

namespace InventorySystem.ViewModels;

public class ImportSpreadsheetViewModel : ViewModelBase
{
    private ExcelPackage _excelApp;

    private string _filePath;

    private ICommand _importCommand;

    private bool _includeTransactions = true;

    private string _inventorySheetName;

    private ExcelTable _inventoryTable;

    private IEnumerable<ExcelTable> _inventoryTables;

    private IEnumerable<string> _sheetNames;

    private string _transactionSheetName;

    private ExcelTable _transactionTable;

    private IEnumerable<ExcelTable> _transactionTables;

    public ErrorViewModel ErrorViewModelInstance { get; } = new();

    public string FilePath
    {
        get => _filePath;
        set
        {
            SetField(ref _filePath, value);

            _excelApp?.Dispose();
            _excelApp = new ExcelPackage(new FileInfo(FilePath));

            SheetNames = GetSheetNames();
        }
    }

    public IEnumerable<string> SheetNames
    {
        get => _sheetNames;
        private set => SetField(ref _sheetNames, value);
    }

    public string InventorySheetName
    {
        get => _inventorySheetName;
        set
        {
            SetField(ref _inventorySheetName, value);
            InventoryTables = GetTables(InventorySheetName, 5);
        }
    }

    public IEnumerable<ExcelTable> InventoryTables
    {
        get => _inventoryTables;
        private set => SetField(ref _inventoryTables, value);
    }

    public ExcelTable InventoryTable
    {
        get => _inventoryTable;
        set => SetField(ref _inventoryTable, value);
    }

    public bool IncludeTransactions
    {
        get => _includeTransactions;
        set => SetField(ref _includeTransactions, value);
    }

    public string TransactionSheetName
    {
        get => _transactionSheetName;
        set
        {
            SetField(ref _transactionSheetName, value);
            TransactionTables = GetTables(TransactionSheetName, 8);
        }
    }

    public IEnumerable<ExcelTable> TransactionTables
    {
        get => _transactionTables;
        private set => SetField(ref _transactionTables, value);
    }

    public ExcelTable TransactionTable
    {
        get => _transactionTable;
        set => SetField(ref _transactionTable, value);
    }

    public ICommand ImportCommand => _importCommand ??= new RelayCommand(Import, CanImport);

    public event Action SpreadsheetImported;

    private bool CanImport()
    {
        var initial = !string.IsNullOrWhiteSpace(FilePath) && !string.IsNullOrWhiteSpace(InventorySheetName) &&
                      InventoryTables.Any() &&
                      InventoryTable != null;

        if (IncludeTransactions)
        {
            initial = initial && !string.IsNullOrWhiteSpace(TransactionSheetName) && TransactionTables.Any() &&
                      TransactionTable != null;
        }

        return initial;
    }

    private void Import()
    {
        _excelApp ??= new ExcelPackage(new FileInfo(FilePath));

        var inventory = ImportInventory();

        if (ErrorViewModelInstance.HasError) return;

        InventorySingletonViewModel.Instance.Items = new ObservableCollectionWithItemNotify<Item>(inventory);

        if (IncludeTransactions)
        {
            var transactions = ImportTransactions(inventory);

            if (ErrorViewModelInstance.HasError) return;

            TransactionsSingletonViewModel.Instance.Transactions =
                new ObservableCollectionWithItemNotify<Transaction>(transactions);
        }

        ErrorViewModelInstance.HasError = false;

        _excelApp.Dispose();
        _excelApp = null;

        SpreadsheetImported?.Invoke();
    }

    private IEnumerable<Transaction> ImportTransactions(IReadOnlyCollection<Item> inventory)
    {
        var transactions = new List<Transaction>();
        var transactionSheet = _excelApp.Workbook.Worksheets[TransactionSheetName];
        var tableRange = TransactionTable.Range;
        var startRow = TransactionTable.ShowHeader ? tableRange.Start.Row + 1 : tableRange.Start.Row;
        for (var i = startRow; i <= tableRange.End.Row; i++)
        {
            try
            {
                var baseColumn = tableRange.Start.Column;
                var itemId = Convert.ToInt32(transactionSheet.Cells[i, baseColumn + 2].Value);
                var transaction = new Transaction
                {
                    Id = Guid.Parse(Convert.ToString(transactionSheet.Cells[i, baseColumn + 0].Value)),
                    Date = DateTime.Parse(Convert.ToString(transactionSheet.Cells[i, baseColumn + 1].Value)),
                    Item = inventory.First(item => item.Id == itemId),
                    StockIn = Convert.ToInt32(transactionSheet.Cells[i, baseColumn + 3].Value),
                    StockOut = Convert.ToInt32(transactionSheet.Cells[i, baseColumn + 4].Value),
                    Status = (TransactionStatus)Enum.Parse(typeof(TransactionStatus),
                        Convert.ToString(transactionSheet.Cells[i, baseColumn + 5].Value)),
                    TotalPrice = Convert.ToDecimal(transactionSheet.Cells[i, baseColumn + 6].Value),
                    Notes = Convert.ToString(transactionSheet.Cells[i, baseColumn + 7].Value)
                };
                transactions.Insert(0, transaction);
            }
            catch (Exception e)
            {
                ErrorViewModelInstance.Error = $"Error at row {i}: {e.Message}";
                ErrorViewModelInstance.HasError = true;
                return transactions;
            }
        }

        return transactions;
    }

    private List<Item> ImportInventory()
    {
        var inventory = new List<Item>();
        var inventorySheet = _excelApp.Workbook.Worksheets[InventorySheetName];
        var tableRange = InventoryTable.Range;
        var startRow = InventoryTable.ShowHeader ? tableRange.Start.Row + 1 : tableRange.Start.Row;
        for (var i = startRow; i <= tableRange.End.Row; i++)
        {
            try
            {
                var baseColumn = tableRange.Start.Column;
                var item = new Item
                {
                    Id = Convert.ToInt32(inventorySheet.Cells[i, baseColumn + 0].Value),
                    Name = Convert.ToString(inventorySheet.Cells[i, baseColumn + 1].Value),
                    Description = Convert.ToString(inventorySheet.Cells[i, baseColumn + 2].Value),
                    Quantity = Convert.ToInt32(inventorySheet.Cells[i, baseColumn + 3].Value),
                    Price = Convert.ToDecimal(inventorySheet.Cells[i, baseColumn + 4].Value)
                };
                inventory.Add(item);
            }
            catch (Exception e)
            {
                ErrorViewModelInstance.Error = $"Error at row {i}: {e.Message}";
                ErrorViewModelInstance.HasError = true;
                return inventory;
            }
        }

        return inventory;
    }

    private IEnumerable<string> GetSheetNames()
    {
        var sheets = _excelApp.Workbook.Worksheets;
        return sheets.Select(sheet => sheet.Name).ToList();
    }

    private IEnumerable<ExcelTable> GetTables(string sheetName, int columnCount)
    {
        var sheet = _excelApp.Workbook.Worksheets[sheetName];
        return sheet.Tables.Where(table => table.Columns.Count == columnCount);
    }
}