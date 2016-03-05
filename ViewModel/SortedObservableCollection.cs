using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Sbs20.Actiontext.ViewModel
{
    public class SortedObservableCollection<T> : ObservableCollection<T>
    {
        public Func<T, IComparable> SortKey { get; set; }

        public SortedObservableCollection(List<T> list)
            : base(list)
        {
            this.SortKey = i => 0;
        }

        public SortedObservableCollection()
        {
            this.SortKey = i => 0;
        }

        public new void Add(T item)
        {
            var itemAfter = this.FirstOrDefault(x => this.SortKey(x).CompareTo(this.SortKey(item)) > 0);
            if (itemAfter == null)
            {
                base.Add(item);
            }
            else
            {
                int index = this.IndexOf(itemAfter);
                base.Insert(index, item);
            }
        }

        public new void Insert(int index, T item)
        {
            throw new InvalidOperationException("Cannot insert into sorted collection. Use Add()");
        }

        public void Sort()
        {
            ApplySort(Items.OrderBy(this.SortKey));
        }

        public void Sort<TKey>(Func<T, TKey> keySelector)
        {
            ApplySort(Items.OrderBy(keySelector));
        }

        public void Sort<TKey>(Func<T, TKey> keySelector, IComparer<TKey> comparer)
        {
            ApplySort(Items.OrderBy(keySelector, comparer));
        }

        private void ApplySort(IEnumerable<T> sortedItems)
        {
            var sortedItemsList = sortedItems.ToList();

            for (int index = 0; index < sortedItemsList.Count; index++)
            {
                var item = sortedItemsList[index];
                int old = IndexOf(item);
                if (old != index)
                {
                    this.Move(old, index);
                }
            }
        }
    }
}
