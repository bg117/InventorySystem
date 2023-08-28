using System.Windows.Input;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace InventorySystem.ViewModels;

public class InventoryFilterViewModel : ViewModelBase
{
    [Reactive] public string? SearchQuery { get; set; }
    [Reactive] public int? QuantityRangeLowerBound { get; set; }
    [Reactive] public int? QuantityRangeUpperBound { get; set; }
    [Reactive] public decimal? PriceRangeLowerBound { get; set; }
    [Reactive] public decimal? PriceRangeUpperBound { get; set; }
    
    public ICommand ClearSearchQueryCommand => ReactiveCommand.Create(ClearSearchQuery, this.WhenAnyValue(
        x => x.SearchQuery,
        query => !string.IsNullOrEmpty(query)
    ));

    public ICommand ClearQuantityRangeCommand => ReactiveCommand.Create(ClearQuantityRange, this.WhenAnyValue(
        x => x.QuantityRangeLowerBound,
        x => x.QuantityRangeUpperBound,
        (lowerBound, upperBound) => lowerBound is not null || upperBound is not null
    ));

    public ICommand ClearPriceRangeCommand => ReactiveCommand.Create(ClearPriceRange, this.WhenAnyValue(
        x => x.PriceRangeLowerBound,
        x => x.PriceRangeUpperBound,
        (lowerBound, upperBound) => lowerBound is not null || upperBound is not null
    ));
    
    private void ClearSearchQuery()
    {
        SearchQuery = null;
    }

    private void ClearQuantityRange()
    {
        QuantityRangeLowerBound = null;
        QuantityRangeUpperBound = null;
    }

    private void ClearPriceRange()
    {
        PriceRangeLowerBound = null;
        PriceRangeUpperBound = null;
    }
}