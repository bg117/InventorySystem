using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Windows.Input;
using DynamicData;
using DynamicData.Binding;
using InventorySystem.Models;
using InventorySystem.ViewModels.Singleton;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace InventorySystem.ViewModels;

public class InventoryViewModel : ViewModelBase
{
    private readonly ReadOnlyObservableCollection<Item> _filteredItems;
    public ReadOnlyObservableCollection<Item> FilteredItems => _filteredItems;

    public InventoryViewModel()
    {
        var filter = this.WhenAnyValue(
            x => x.SearchQuery,
            x => x.QuantityRangeLowerBound,
            x => x.QuantityRangeUpperBound,
            x => x.PriceRangeLowerBound,
            x => x.PriceRangeUpperBound,
            (query, quantityLowerBound, quantityUpperBound, priceLowerBound, priceUpperBound) =>
                new Func<Item, bool>(item =>
                    (string.IsNullOrEmpty(query) || item.Name.Contains(query) ||
                     item.Description?.Contains(query) == true) &&
                    (quantityLowerBound is null || item.Quantity >= quantityLowerBound) &&
                    (quantityUpperBound is null || item.Quantity <= quantityUpperBound) &&
                    (priceLowerBound is null || item.UnitPrice >= priceLowerBound) &&
                    (priceUpperBound is null || item.UnitPrice <= priceUpperBound)
                )
        );

        Context.Instance.Items
            .Connect()
            .Filter(filter)
            .Sort(SortExpressionComparer<Item>.Ascending(item => item.Id))
            .ObserveOn(RxApp.MainThreadScheduler)
            .Bind(out _filteredItems)
            .Subscribe();
    }

    [Reactive] public bool IsFilterVisible { get; set; }
    [Reactive] public string? SearchQuery { get; set; }
    [Reactive] public int? QuantityRangeLowerBound { get; set; }
    [Reactive] public int? QuantityRangeUpperBound { get; set; }
    [Reactive] public decimal? PriceRangeLowerBound { get; set; }
    [Reactive] public decimal? PriceRangeUpperBound { get; set; }

    public ICommand FilterCommand => ReactiveCommand.Create(Filter);
    public ICommand AddItemCommand => ReactiveCommand.Create(AddItem);
    public ICommand RemoveItemCommand => ReactiveCommand.Create(RemoveItem);

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

    private void Filter()
    {
        IsFilterVisible = !IsFilterVisible;
    }

    private static int _id = 5;

    private void AddItem()
    {
        // add dummy data
        var id = ++_id;
        Context.Instance.Items.Add(new Item
        {
            Name = $"Item {id}",
            Description = $"Description {id}",
            Quantity = id,
            UnitPrice = id * 10
        });
    }

    private void RemoveItem()
    {
        throw new NotImplementedException();
    }

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