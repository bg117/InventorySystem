using InventorySystem.Common;

namespace InventorySystem.Models;

public class Item : ViewModelBase
{
    private static int _idBase = 1000000000;

    private string _description;
    private int _id;

    private string _name;

    private decimal _price;

    private int _quantity;

    public int Id
    {
        get => _id;
        set => SetField(ref _id, value);
    }

    public string Name
    {
        get => _name;
        set => SetField(ref _name, value);
    }

    public int Quantity
    {
        get => _quantity;
        set => SetField(ref _quantity, value);
    }

    public string Description
    {
        get => _description;
        set => SetField(ref _description, value);
    }

    public decimal Price
    {
        get => _price;
        set => SetField(ref _price, value);
    }

    public static int NewId()
    {
        var id = _idBase;
        _idBase += 10;
        return id;
    }
}