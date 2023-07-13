using InventorySystem.Attributes;
using InventorySystem.Interfaces;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Model;

namespace InventorySystem.Models;

[NotifyPropertyChanged]
public class Item
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }
    
    public int Quantity { get; set; }
    
    public string Description { get; set; }
    
    public decimal Price { get; set; }

    public static int IdBase { private get; set; } = 1000000000;

    public static int IdInterval { private get; set; } = 10;

    public static int NewId()
    {
        var id = IdBase;
        IdBase += IdInterval;
        return id;
    }
}