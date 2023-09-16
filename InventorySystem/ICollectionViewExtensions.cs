using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventorySystem;
internal static class ICollectionViewExtensions
{
    internal static void SafeRefresh(this ICollectionView collectionView)
    {
        if (collectionView is not IEditableCollectionView editable)
        {
            collectionView.Refresh();
            return;
        }

        if (editable.IsAddingNew)
            editable.CommitNew();

        if (editable.IsEditingItem)
            editable.CommitEdit();

        collectionView.Refresh();
    }
}
