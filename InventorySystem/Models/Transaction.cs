using System;
using Newtonsoft.Json;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace InventorySystem.Models;

[JsonObject(MemberSerialization.OptIn)]
public class Transaction : ReactiveObject
{
    [JsonProperty] [Reactive] public Guid Id { get; set; }
    [JsonProperty] [Reactive] public DateTime Date { get; set; }
    [JsonProperty] [Reactive] public Item Item { get; set; }
    [JsonProperty] [Reactive] public int In { get; set; }
    [JsonProperty] [Reactive] public int Out { get; set; }
    [JsonProperty] [Reactive] public decimal Total { get; set; }
    [JsonProperty] [Reactive] public string? Remarks { get; set; }
}