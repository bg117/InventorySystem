using InventorySystem.Models;
using InventorySystem.Common;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows.Input;
using System;

namespace InventorySystem.ViewModels;

public class ImportSpreadsheetViewModel : ObservableObject, IDisposable
{
    private string _filePath;
    public string FilePath
    {
        get => _filePath;
        set
        {
            SetField(ref _filePath, value);

            if (_workbook != null)
            {
                _workbook.Close();
                Marshal.ReleaseComObject(_workbook);
            }

            _workbook = _workbooks.Open(FilePath);
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

    private readonly Excel.Application _excelApp;
    private readonly Excel.Workbooks _workbooks;
    private Excel.Workbook _workbook;

    public ImportSpreadsheetViewModel()
    {
        _excelApp = new Excel.Application();
        _workbooks = _excelApp.Workbooks;
    }

    private IEnumerable<string> GetSheetNames()
    {
        var sheets = _workbook.Sheets;
        var sheetNames = new List<string>();

        foreach (Excel.Worksheet sheet in sheets)
        {
            sheetNames.Add(sheet.Name);
        }

        Marshal.ReleaseComObject(sheets);

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

    private void Import()
    {
        var inventorySheet = _workbook.Sheets[InventorySheetName];
        var inventoryRange = inventorySheet.Range[InventoryCellRange.StartCell, InventoryCellRange.EndCell];
        var inventoryValues = inventoryRange.Value;

        var inventory = new List<Item>();
        for (int i = 1; i < inventoryValues.GetLength(0); i++)
        {
            var item = new Item
            {
                Id = Convert.ToInt32(inventoryValues[i, 1]),
                Name = inventoryValues[i, 2],
                Description = inventoryValues[i, 3],
                Quantity = Convert.ToInt32(inventoryValues[i, 4]),
                Price = Convert.ToDecimal(inventoryValues[i, 5])
            };
            inventory.Add(item);
        }

        InventorySingletonViewModel.Items = new ObservableCollectionWithItemNotify<Item>(inventory);

        Marshal.ReleaseComObject(inventorySheet);

        if (!IncludeTransactions)
            return;

        var transactionSheet = _workbook.Sheets[TransactionSheetName];
        var transactionRange = transactionSheet.Range[TransactionCellRange.StartCell, TransactionCellRange.EndCell];
        var transactionValues = transactionRange.Value;

        var transactions = new List<Transaction>();
        for (int i = 1; i < transactionValues.GetLength(0); i++)
        {
            var itemId = Convert.ToInt32(transactionValues[i, 3]);
            var item = inventory.Find(item => item.Id == itemId);
            transactions.Add(new Transaction
            {
                Id = Guid.Parse(transactionValues[i, 1]),
                Date = DateTime.Parse(transactionValues[i, 2]),
                Item = item,
                StockIn = Convert.ToInt32(transactionValues[i, 4]),
                StockOut = Convert.ToInt32(transactionValues[i, 5]),
                Status = (TransactionStatus)Enum.Parse(typeof(TransactionStatus), transactionValues[i, 6]),
                TotalPrice = Convert.ToDecimal(transactionValues[i, 7]),
                Notes = transactionValues[i, 8] ?? string.Empty
            });
        }

        TransactionsSingletonViewModel.Transactions = new ObservableCollectionWithItemNotify<Transaction>(transactions);

        Marshal.ReleaseComObject(transactionSheet);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects)
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
            _disposedValue = true;

            if (_workbook != null)
            {
                _workbook.Close();
                Marshal.ReleaseComObject(_workbook);
            }

            _workbooks.Close();
            _excelApp.Quit();

            Marshal.ReleaseComObject(_workbooks);
            Marshal.ReleaseComObject(_excelApp);
        }
    }

    // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    ~ImportSpreadsheetViewModel()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: false);
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
