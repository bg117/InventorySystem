using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;
using InventorySystem.Common;
using InventorySystem.Models;
using OfficeOpenXml;

namespace InventorySystem.ViewModels;

public class ImportSpreadsheetViewModel : ViewModelBase
{
    private ExcelPackage _excelApp;

    private string _filePath;

    private ICommand _importCommand;

    private bool _includeTransactions = true;

    private int _inventoryRows = 1;

    private string _inventorySheetName;

    private string _inventoryStartCell;

    private IEnumerable<string> _sheetNames;

    private int _transactionRows = 1;

    private string _transactionSheetName;

    private string _transactionStartCell;

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

            ErrorViewModelInstance.HasError = false;
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
        set => SetField(ref _inventorySheetName, value);
    }

    public string InventoryStartCell
    {
        get => _inventoryStartCell;
        set => SetField(ref _inventoryStartCell, value);
    }

    public int InventoryRows
    {
        get => _inventoryRows;
        set => SetField(ref _inventoryRows, value);
    }

    public bool IncludeTransactions
    {
        get => _includeTransactions;
        set => SetField(ref _includeTransactions, value);
    }

    public string TransactionSheetName
    {
        get => _transactionSheetName;
        set => SetField(ref _transactionSheetName, value);
    }

    public string TransactionStartCell
    {
        get => _transactionStartCell;
        set => SetField(ref _transactionStartCell, value);
    }

    public int TransactionRows
    {
        get => _transactionRows;
        set => SetField(ref _transactionRows, value);
    }

    public ICommand ImportCommand => _importCommand ??= new RelayCommand(Import, CanImport);

    public event Action SpreadsheetImported;


    private bool CanImport()
    {
        var initial = !string.IsNullOrWhiteSpace(FilePath) && !string.IsNullOrWhiteSpace(InventorySheetName) &&
                      !string.IsNullOrWhiteSpace(InventoryStartCell) && InventoryRows > 0;

        if (IncludeTransactions)
            initial = initial && !string.IsNullOrWhiteSpace(TransactionSheetName) &&
                      !string.IsNullOrWhiteSpace(TransactionStartCell) && TransactionRows > 0;

        return initial;
    }

    private void Import()
    {
        _excelApp ??= new ExcelPackage(new FileInfo(FilePath));

        var inventory = ImportInventory();
        InventorySingletonViewModel.Instance.Items = new ObservableCollectionWithItemNotify<Item>(inventory);

        if (IncludeTransactions)
        {
            var transactions = ImportTransactions(inventory);

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
        var transactionSheet = _excelApp.Workbook.Worksheets[TransactionSheetName];
        var transactionStartCell = GetRowAndColumn(TransactionStartCell);

        var transactions = new List<Transaction>();
        for (var i = transactionStartCell.Item1; i < transactionStartCell.Item1 + TransactionRows; i++)
            try
            {
                var baseColumn = transactionStartCell.Item2;
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

        return transactions;
    }

    private List<Item> ImportInventory()
    {
        var inventorySheet = _excelApp.Workbook.Worksheets[InventorySheetName];
        var inventoryStartCell = GetRowAndColumn(InventoryStartCell);

        var inventory = new List<Item>();
        for (var i = inventoryStartCell.Item1; i < inventoryStartCell.Item1 + InventoryRows; i++)
            try
            {
                var baseColumn = inventoryStartCell.Item2;
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

        return inventory;
    }

    private IEnumerable<string> GetSheetNames()
    {
        var sheets = _excelApp.Workbook.Worksheets;
        return sheets.Select(sheet => sheet.Name).ToList();
    }

    private static (int, int) GetRowAndColumn(string cell)
    {
        // get the column from the cell string e.g. B3 => (3, 2)
        var regex = new Regex(@"([A-Z]+)(\d+)");
        var match = regex.Match(cell);
        var column = match.Groups[1].Value;
        var row = match.Groups[2].Value;

        // convert column to number e.g. A => 1, B => 2, AA => 27
        column = column.ToUpper();
        var sum = column.Select(c => c - 'A' + 1).Select((value, i) => value * (int)Math.Pow(26, column.Length - i - 1))
            .Sum();

        return (Convert.ToInt32(row), sum);
    }
}