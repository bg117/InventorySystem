using InventorySystem.Models;
using InventorySystem.Common;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Input;
using System;
using OfficeOpenXml;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;

namespace InventorySystem.ViewModels;

public class ImportSpreadsheetViewModel : ObservableObject
{
    private string _filePath;
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

    private IEnumerable<string> _sheetNames;
    public IEnumerable<string> SheetNames
    {
        get => _sheetNames;
        set => SetField(ref _sheetNames, value);
    }

    private string _inventorySheetName;
    public string InventorySheetName
    {
        get => _inventorySheetName;
        set => SetField(ref _inventorySheetName, value);
    }

    private CellRange _inventoryCellRange = new();
    public CellRange InventoryCellRange
    {
        get => _inventoryCellRange;
        set => SetField(ref _inventoryCellRange, value);
    }

    private bool _includeTransactions = true;
    public bool IncludeTransactions
    {
        get => _includeTransactions;
        set => SetField(ref _includeTransactions, value);
    }

    private string _transactionSheetName;
    public string TransactionSheetName
    {
        get => _transactionSheetName;
        set => SetField(ref _transactionSheetName, value);
    }

    private CellRange _transactionCellRange = new();
    public CellRange TransactionCellRange
    {
        get => _transactionCellRange;
        set => SetField(ref _transactionCellRange, value);
    }

    private ExcelPackage _excelApp;

    private IEnumerable<string> GetSheetNames()
    {
        var sheets = _excelApp.Workbook.Worksheets;
        var sheetNames = new List<string>();

        foreach (var sheet in sheets)
        {
            sheetNames.Add(sheet.Name);
        }

        return sheetNames;
    }

    private ICommand _importCommand;
    private bool _disposedValue;

    public ICommand ImportCommand => _importCommand ??= new RelayCommand(Import, CanImport);

    private InventorySingletonViewModel InventorySingletonViewModel => InventorySingletonViewModel.Instance;
    private TransactionsSingletonViewModel TransactionsSingletonViewModel => TransactionsSingletonViewModel.Instance;

    private bool CanImport()
    {
        var initial = !string.IsNullOrWhiteSpace(FilePath) && !string.IsNullOrWhiteSpace(InventorySheetName) && InventoryCellRange != null;
        if (IncludeTransactions)
        {
            initial = initial && !string.IsNullOrWhiteSpace(TransactionSheetName) && TransactionCellRange != null;
        }

        return initial;
    }

    private (int, int) GetRowAndColumn(string cell)
    {
        // get the column from the cell string e.g. B3 => (3, 2)
        var regex = new Regex(@"([A-Z]+)(\d+)");
        var match = regex.Match(cell);
        var column = match.Groups[1].Value;
        var row = match.Groups[2].Value;

        // convert column to number e.g. A => 1, B => 2, AA => 27
        column = column.ToUpper();
        var sum = 0;
        for (int i = 0; i < column.Length; i++)
        {
            var c = column[i];
            var value = c - 'A' + 1;
            sum += value * (int)Math.Pow(26, column.Length - i - 1);
        }

        return (Convert.ToInt32(row), sum);
    }

    private void Import()
    {
        var inventorySheet = _excelApp.Workbook.Worksheets[InventorySheetName];
        var inventoryStartCell = GetRowAndColumn(InventoryCellRange.StartCell);
        var inventoryEndCell = GetRowAndColumn(InventoryCellRange.EndCell);

        var inventory = new List<Item>();
        for (int i = inventoryStartCell.Item1; i <= inventoryEndCell.Item1; i++)
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

        InventorySingletonViewModel.Items = new ObservableCollectionWithItemNotify<Item>(inventory);

        if (IncludeTransactions)
        {
            var transactionSheet = _excelApp.Workbook.Worksheets[TransactionSheetName];
            var transactionStartCell = GetRowAndColumn(TransactionCellRange.StartCell);
            var transactionEndCell = GetRowAndColumn(TransactionCellRange.EndCell);

            var transactions = new List<Transaction>();
            for (int i = transactionStartCell.Item1; i <= transactionEndCell.Item1; i++)
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
                    Status = (TransactionStatus)Enum.Parse(typeof(TransactionStatus), Convert.ToString(transactionSheet.Cells[i, baseColumn + 5].Value)),
                    TotalPrice = Convert.ToDecimal(transactionSheet.Cells[i, baseColumn + 6].Value),
                    Notes = Convert.ToString(transactionSheet.Cells[i, baseColumn + 7].Value)
                };
                transactions.Insert(0, transaction);
            }

            TransactionsSingletonViewModel.Transactions = new ObservableCollectionWithItemNotify<Transaction>(transactions);
        }

        _excelApp?.Dispose();
    }
}
