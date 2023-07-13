using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using InventorySystem.Models;
using OfficeOpenXml;
using PostSharp.Patterns.Model;
using PostSharp.Patterns.Xaml;

namespace InventorySystem.ViewModels;

[NotifyPropertyChanged]
[Disposable]
public sealed class ImportSpreadsheetViewModel : INotifyPropertyChanged
{
    private string _filePath;
    private ExcelPackage _spreadsheet;
    public ErrorViewModel ErrorViewModelInstance { get; } = new();

    [SafeForDependencyAnalysis] private ExcelPackage Spreadsheet => GetSpreadsheet();

    [IgnoreAutoChangeNotification]
    public string FilePath
    {
        get => _filePath;
        set
        {
            SetField(ref _filePath, value);
            OnPropertyChanged(nameof(Spreadsheet));
            OnPropertyChanged(nameof(SheetNames));
        }
    }

    public List<string> SheetNames =>
        GetSheetNames();

    public string InventorySheetName { get; set; }

    public List<string> InventoryTableNames => GetTableNames(InventorySheetName, 5);

    public string InventoryTableName { get; set; }

    public bool IncludeTransactions { get; set; } = true;

    public string TransactionSheetName { get; set; }

    public List<string> TransactionTableNames => GetTableNames(TransactionSheetName, 8);

    public string TransactionTableName { get; set; }

    [Command] public ICommand ImportCommand { get; }

    public bool CanExecuteImport
    {
        get
        {
            var initial = !string.IsNullOrWhiteSpace(FilePath) && !string.IsNullOrWhiteSpace(InventorySheetName) &&
                          InventoryTableNames.Count > 0 &&
                          !string.IsNullOrWhiteSpace(InventoryTableName);

            if (IncludeTransactions)
                initial = initial && !string.IsNullOrWhiteSpace(TransactionSheetName) &&
                          TransactionTableNames.Count > 0 &&
                          !string.IsNullOrWhiteSpace(TransactionTableName);

            return initial;
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    [Pure]
    private ExcelPackage GetSpreadsheet()
    {
        try
        {
            _spreadsheet ??= new ExcelPackage(new FileInfo(FilePath));

            try
            {
                _ = _spreadsheet.Workbook;
            }
            catch (ObjectDisposedException)
            {
                _spreadsheet = new ExcelPackage(new FileInfo(FilePath));
            }

            return _spreadsheet;
        }
        catch (ArgumentNullException)
        {
            return null;
        }
    }

    [Pure]
    private List<string> GetSheetNames()
    {
        return Spreadsheet?.Workbook.Worksheets.Select(x => x.Name).ToList() ?? new List<string>();
    }

    public event Action ImportCompleted;

    public void ExecuteImport()
    {
        var inventory = ImportInventory();

        if (ErrorViewModelInstance.HasError) return;

        InventorySingletonViewModel.Instance.Items = new ObservableCollection<Item>(inventory);

        if (IncludeTransactions)
        {
            var transactions = ImportTransactions(inventory);

            if (ErrorViewModelInstance.HasError) return;

            TransactionsSingletonViewModel.Instance.Transactions =
                new ObservableCollection<Transaction>(transactions);
        }

        ErrorViewModelInstance.HasError = false;

        var singleton = SpreadsheetSingletonViewModel.Instance;
        singleton.FilePath = FilePath;
        singleton.InventorySheetName = InventorySheetName;
        singleton.InventoryTableName = InventoryTableName;
        singleton.TransactionSheetName = TransactionSheetName;
        singleton.TransactionTableName = TransactionTableName;

        Spreadsheet.Dispose();

        var idList = inventory.Select(x => x.Id).ToArray();

        var idBase = idList.Min();

        if (idList.Length > 1)
        {
            var idTop = idList.Max();
            var idRange = idTop - idBase;

            var idInterval = idRange / (idList.Length > 1 ? idList.Length - 1 : 1);
            idBase = idTop + idInterval;
            Item.IdInterval = idInterval;
        }

        Item.IdBase = idBase;

        ImportCompleted?.Invoke();
    }

    private IEnumerable<Transaction> ImportTransactions(IReadOnlyCollection<Item> inventory)
    {
        var transactionSheet = Spreadsheet.Workbook.Worksheets[TransactionSheetName];
        var transactionTable = transactionSheet.Tables[TransactionTableName];
        var transactions = new List<Transaction>();
        var tableRange = transactionTable.Range;
        var startRow = transactionTable.ShowHeader ? tableRange.Start.Row + 1 : tableRange.Start.Row;
        for (var i = startRow; i <= tableRange.End.Row; i++)
            try
            {
                var baseColumn = tableRange.Start.Column;
                var itemId = Convert.ToInt32(transactionSheet.Cells[i, baseColumn + 2].Value);
                var transaction = new Transaction
                {
                    Id = Guid.Parse(Convert.ToString(transactionSheet.Cells[i, baseColumn + 0].Value) ??
                                    string.Empty),
                    Date = DateTime.FromOADate(Convert.ToDouble(transactionSheet.Cells[i, baseColumn + 1].Value)),
                    Item = inventory.First(item => item.Id == itemId),
                    StockIn = Convert.ToInt32(transactionSheet.Cells[i, baseColumn + 3].Value),
                    StockOut = Convert.ToInt32(transactionSheet.Cells[i, baseColumn + 4].Value),
                    Status = (TransactionStatus)Enum.Parse(typeof(TransactionStatus),
                        Convert.ToString(transactionSheet.Cells[i, baseColumn + 5].Value) ?? string.Empty),
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
        var inventorySheet = Spreadsheet.Workbook.Worksheets[InventorySheetName];
        var inventoryTable = inventorySheet.Tables[InventoryTableName];
        var inventory = new List<Item>();
        var tableRange = inventoryTable.Range;
        var startRow = inventoryTable.ShowHeader ? tableRange.Start.Row + 1 : tableRange.Start.Row;
        for (var i = startRow; i <= tableRange.End.Row; i++)
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

        return inventory;
    }

    [Pure]
    private List<string> GetTableNames(string sheetName, int columnCount)
    {
        if (string.IsNullOrWhiteSpace(sheetName))
            return new List<string>();

        var worksheet = Spreadsheet.Workbook.Worksheets[sheetName];
        return worksheet.Tables.Where(table => table.Columns.Count == columnCount)
            .Select(table => table.Name).ToList();
    }

    private void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return;
        field = value;
        OnPropertyChanged(propertyName);
    }
}