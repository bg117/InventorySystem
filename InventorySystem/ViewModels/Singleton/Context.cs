using System;
using System.Text.Json.Serialization;
using DynamicData;
using InventorySystem.Models;

namespace InventorySystem.ViewModels.Singleton;

public class Context
{
    public static Context Instance { get; } = new();

    private Context()
    {
    }

    public SourceCache<Item, int> Items { get; } = new(item => item.Id);
    public SourceCache<Transaction, Guid> Transactions { get; } = new(transaction => transaction.Id);
}