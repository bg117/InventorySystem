using PostSharp.Patterns.Model;

namespace InventorySystem.ViewModels;

[NotifyPropertyChanged]
public class MainWindowViewModel
{
    private readonly InventorySingletonViewModel _inventorySingletonViewModel = InventorySingletonViewModel.Instance;

    private readonly SpreadsheetSingletonViewModel _spreadsheetSingletonViewModel =
        SpreadsheetSingletonViewModel.Instance;

    private readonly TransactionsSingletonViewModel _transactionsSingletonViewModel =
        TransactionsSingletonViewModel.Instance;

    public string FilePathDisplay => $"— {_spreadsheetSingletonViewModel.FilePath ?? "<untitled>"}";

    public string ChangedDisplay =>
        _inventorySingletonViewModel.IsChanged || _transactionsSingletonViewModel.IsChanged ? "*" : "";
}