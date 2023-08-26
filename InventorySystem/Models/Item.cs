using System;
using Newtonsoft.Json;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace InventorySystem.Models;

[JsonObject(MemberSerialization.OptIn)]
public class Item : ReactiveObject
{
    [JsonProperty] [Reactive] public int Id { get; set; }
    [JsonProperty] [Reactive] public string Name { get; set; }
    [JsonProperty] [Reactive] public string Description { get; set; }
    [JsonProperty] [Reactive] public int Quantity { get; set; }
    [JsonProperty] [Reactive] public decimal UnitPrice { get; set; }

    public Item()
    {
        Name = string.Empty;
        Description = string.Empty;
    }
}