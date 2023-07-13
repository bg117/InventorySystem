using JetBrains.Annotations;
using PostSharp.Patterns.Model;

namespace InventorySystem.ViewModels;

[NotifyPropertyChanged]
public class SpreadsheetSingletonViewModel
{
    private SpreadsheetSingletonViewModel()
    {
    }

    public static SpreadsheetSingletonViewModel Instance { get; } = new();

    public string FilePath { [UsedImplicitly] get; set; }

    public string InventoryTableName { get; set; }

    public string TransactionTableName { get; set; }

    public string InventorySheetName { get; set; }

    public string TransactionSheetName { get; set; }
}