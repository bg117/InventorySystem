using InventorySystem.Common;

namespace InventorySystem.ViewModels;

public class ErrorViewModel : ViewModelBase
{
    private string _error;

    private bool _hasError;

    public string Error
    {
        get => _error;
        set => SetField(ref _error, value);
    }

    public bool HasError
    {
        get => _hasError;
        set => SetField(ref _hasError, value);
    }
}