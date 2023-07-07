using InventorySystem.Models;
using InventorySystem.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventorySystem.ViewModels;

public class ImportSpreadsheetViewModel : ObservableObject
{
    private string _filePath;
    public string FilePath
    {
        get => _filePath;
        set => SetField(ref _filePath, value);
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

    private CellRange _inventoryCellRange;
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

    private CellRange _transactionCellRange;
    public CellRange TransactionCellRange
    {
        get => _transactionCellRange;
        set => SetField(ref _transactionCellRange, value);
    }
}
