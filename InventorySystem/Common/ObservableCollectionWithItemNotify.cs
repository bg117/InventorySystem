using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace InventorySystem.Common
{
    public sealed class ObservableCollectionWithItemNotify<T> : ObservableCollection<T> where T : INotifyPropertyChanged
    {

        public ObservableCollectionWithItemNotify()
        {
            CollectionChanged += items_CollectionChanged;
        }


        public ObservableCollectionWithItemNotify(IEnumerable<T> collection) : base(collection)
        {
            CollectionChanged += items_CollectionChanged;
            foreach (var item in collection)
                item.PropertyChanged += item_PropertyChanged;

        }

        private void items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e == null)
                return;

            if (e.OldItems != null)
                foreach (INotifyPropertyChanged item in e.OldItems)
                    item.PropertyChanged -= item_PropertyChanged;

            if (e.NewItems == null)
                return;

            foreach (INotifyPropertyChanged item in e.NewItems)
                item.PropertyChanged += item_PropertyChanged;
        }

        private void item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var reset = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
            OnCollectionChanged(reset);
        }

    }
}
