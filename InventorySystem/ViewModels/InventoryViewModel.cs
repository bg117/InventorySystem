using System;
using System.Linq;
using System.Reactive.Linq;
using Newtonsoft.Json;
using System.Windows.Input;
using DynamicData;
using DynamicData.Binding;
using InventorySystem.Models;
using InventorySystem.ViewModels.Singleton;
using Newtonsoft.Json.Serialization;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace InventorySystem.ViewModels;

public class InventoryViewModel : ViewModelBase
{
    public InventoryFilterViewModel FilterViewModel { get; } = new();
    public IObservableCollection<Item> FilteredItems { get; } = new ObservableCollectionExtended<Item>();

    public InventoryViewModel()
    {
        Context.Instance.Items.AddOrUpdate(
            new Item
            {
                Id = 1,
                Name = "Item 1",
                Description = "Description 1",
                Quantity = 1,
                UnitPrice = 1.00m
            }
        );

        var filter = FilterViewModel.WhenAnyValue(
            x => x.SearchQuery,
            x => x.QuantityRangeLowerBound,
            x => x.QuantityRangeUpperBound,
            x => x.PriceRangeLowerBound,
            x => x.PriceRangeUpperBound,
            (query, quantityLowerBound, quantityUpperBound, priceLowerBound, priceUpperBound) =>
                new Func<Item, bool>(item =>
                    (string.IsNullOrEmpty(query) ||
                     item.Name.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                     item.Description.Contains(query, StringComparison.OrdinalIgnoreCase)) &&
                    (quantityLowerBound is null || item.Quantity >= quantityLowerBound) &&
                    (quantityUpperBound is null || item.Quantity <= quantityUpperBound) &&
                    (priceLowerBound is null || item.UnitPrice >= priceLowerBound) &&
                    (priceUpperBound is null || item.UnitPrice <= priceUpperBound)
                )
        );

        Context.Instance.Items
            .Connect()
            .AutoRefresh()
            .Filter(filter)
            .Sort(SortExpressionComparer<Item>.Ascending(item => item.Id))
            .ObserveOn(RxApp.MainThreadScheduler)
            .Bind(FilteredItems)
            .Subscribe();
    }

    [Reactive] public bool IsFilterVisible { get; set; }
    [Reactive] public bool IsAddItemVisible { get; set; }

    [Reactive] public string ProductName { get; set; } = string.Empty;
    [Reactive] public string ProductDescription { get; set; } = string.Empty;
    [Reactive] public int ProductInitialQuantity { get; set; } = 1;
    [Reactive] public decimal ProductUnitPrice { get; set; }

    public ICommand OpenFilterCommand => ReactiveCommand.Create(OpenFilter);

    public ICommand OpenAddItemCommand => ReactiveCommand.Create(OpenAddItem);

    public ICommand AddItemCommand => ReactiveCommand.Create(AddItem, this.WhenAnyValue(
        x => x.ProductName,
        x => x.ProductInitialQuantity,
        (name, quantity) => !string.IsNullOrEmpty(name) && quantity > 0
    ));

    public ICommand RemoveItemCommand => ReactiveCommand.Create(RemoveItem);

    

    private void OpenFilter()
    {
        IsFilterVisible = !IsFilterVisible;
    }

    private void OpenAddItem()
    {
        IsAddItemVisible = !IsAddItemVisible;
    }

    private void AddItem()
    {
        // add dummy data
        Context.Instance.Items.AddOrUpdate(new Item
        {
            Id = Context.Instance.Items.Keys.Max() + 1,
            Name = ProductName,
            Description = ProductDescription,
            Quantity = ProductInitialQuantity,
            UnitPrice = ProductUnitPrice
        });
    }

    private void RemoveItem()
    {
        throw new NotImplementedException();
    }
}