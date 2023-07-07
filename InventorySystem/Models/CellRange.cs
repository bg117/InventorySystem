using InventorySystem.Common;

namespace InventorySystem.Models;

public class CellRange : ObservableObject
{
    private string _startCell;
    public string StartCell
    {
        get => _startCell;
        set => SetField(ref _startCell, value);
    }

    private string _endCell;
    public string EndCell
    {
        get => _endCell;
        set => SetField(ref _endCell, value);
    }
}
