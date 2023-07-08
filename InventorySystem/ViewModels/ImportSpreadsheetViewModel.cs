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

public class ImportSpreadsheetViewModel : ViewModelBase
{
    public event Action SpreadsheetImported;

    public ErrorViewModel ErrorViewModelInstance { get; } = new();

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

            ErrorViewModelInstance.HasError = false;
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

    private string _inventoryStartCell;
    public string InventoryStartCell
    {
        get => _inventoryStartCell;
        set => SetField(ref _inventoryStartCell, value);
    }

    private int _inventoryRows = 1;
    public int InventoryRows
    {
        get => _inventoryRows;
        set => SetField(ref _inventoryRows, value);
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

    private string _transactionStartCell;
    public string TransactionStartCell
    {
        get => _transactionStartCell;
        set => SetField(ref _transactionStartCell, value);
    }

    private int _transactionRows = 1;
    public int TransactionRows
    {
        get => _transactionRows;
        set => SetField(ref _transactionRows, value);
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

    public ICommand ImportCommand => _importCommand ??= new RelayCommand(Import, CanImport);

    private InventorySingletonViewModel InventorySingletonViewModel => InventorySingletonViewModel.Instance;
    private TransactionsSingletonViewModel TransactionsSingletonViewModel => TransactionsSingletonViewModel.Instance;


    private bool CanImport()
    {
        var initial = !string.IsNullOrWhiteSpace(FilePath) && !string.IsNullOrWhiteSpace(InventorySheetName) && !string.IsNullOrWhiteSpace(InventoryStartCell) && InventoryRows > 0;
        if (IncludeTransactions)
        {
            initial = initial && !string.IsNullOrWhiteSpace(TransactionSheetName) && !string.IsNullOrWhiteSpace(TransactionStartCell) && TransactionRows > 0;
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
        if (_excelApp == null)
        {
            _excelApp = new ExcelPackage(new FileInfo(FilePath));
        }

        var inventorySheet = _excelApp.Workbook.Worksheets[InventorySheetName];
        var inventoryStartCell = GetRowAndColumn(InventoryStartCell);

        var inventory = new List<Item>();
        for (int i = inventoryStartCell.Item1; i < inventoryStartCell.Item1 + InventoryRows; i++)
        {
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
                return;
            }
        }

        InventorySingletonViewModel.Items = new ObservableCollectionWithItemNotify<Item>(inventory);

        if (IncludeTransactions)
        {
            var transactionSheet = _excelApp.Workbook.Worksheets[TransactionSheetName];
            var transactionStartCell = GetRowAndColumn(TransactionStartCell);

            var transactions = new List<Transaction>();
            for (int i = transactionStartCell.Item1; i < transactionStartCell.Item1 + TransactionRows; i++)
            {
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
                        Status = (TransactionStatus)Enum.Parse(typeof(TransactionStatus), Convert.ToString(transactionSheet.Cells[i, baseColumn + 5].Value)),
                        TotalPrice = Convert.ToDecimal(transactionSheet.Cells[i, baseColumn + 6].Value),
                        Notes = Convert.ToString(transactionSheet.Cells[i, baseColumn + 7].Value)
                    };
                    transactions.Insert(0, transaction);
                }
                catch (Exception e)
                {
                    ErrorViewModelInstance.Error = $"Error at row {i}: {e.Message}";
                    ErrorViewModelInstance.HasError = true;
                    return;
                }
            }

            TransactionsSingletonViewModel.Transactions = new ObservableCollectionWithItemNotify<Transaction>(transactions);
        }

        ErrorViewModelInstance.HasError = false;

        _excelApp.Dispose();
        _excelApp = null;

        SpreadsheetImported?.Invoke();
    }
}
