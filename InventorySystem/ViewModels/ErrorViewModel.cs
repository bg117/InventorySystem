using InventorySystem.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventorySystem.ViewModels;

public class ErrorViewModel : ViewModelBase
{
    private string _error;
    public string Error
    {
        get => _error;
        set => SetField(ref _error, value);
    }

    private bool _hasError;
    public bool HasError
    {
        get => _hasError;
        set => SetField(ref _hasError, value);
    }
}
