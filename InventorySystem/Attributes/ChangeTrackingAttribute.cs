using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using InventorySystem.Interfaces;
using PostSharp.Aspects;
using PostSharp.Serialization;

namespace InventorySystem.Attributes;

[PSerializable]
[AttributeUsage(AttributeTargets.Property)]
public class ChangeTrackingAttribute : LocationInterceptionAspect
{
    public override void OnGetValue(LocationInterceptionArgs args)
    {
        base.OnGetValue(args);
        
        if (args.Value is not INotifyCollectionChanged collection) return;
        if (args.Instance is not IChangeTrackable changeTrackable) return;
        
        collection.CollectionChanged += (_, _) => changeTrackable.IsChanged = true;
    }

    public override void OnSetValue(LocationInterceptionArgs args)
    {
        var oldValue = args.GetCurrentValue();
        base.OnSetValue(args);

        if (args.Instance is not IChangeTrackable changeTrackable) return;
        if (oldValue == args.Value) return;

        changeTrackable.IsChanged = true;
    }
}
