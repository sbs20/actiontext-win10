using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace Sbs20.Actiontext.ViewModel
{
    public class ActionItemCollection : SortedObservableCollection<ActionItem>
    {
        // Singleton
        private static ActionItemCollection instance;

        // Members
        public ObservableCollection<GroupedActionItemCollection> ViewSource { get; private set; }

        private ActionItemCollection()
        {
            this.ViewSource = new ObservableCollection<GroupedActionItemCollection>();
            this.SortKey = actionItem =>
            {
                TimeSpan span = DateTime.MaxValue - actionItem.DisplayDate;
                return (actionItem.IsComplete ? "1" : "0") + ":" + actionItem.Priority + ":" + span.Ticks + ":" + actionItem.Body;
            };

            this.CollectionChanged += TaskCollection_CollectionChanged;
        }

        private void TaskCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //switch (e.Action)
            //{
            //    case NotifyCollectionChangedAction.Add:
            //        foreach (ActionItem todo in e.NewItems)
            //        {
            //            var item = this.ViewSource.Where(i => i.Key == todo.Priority).FirstOrDefault();
            //            if (item == null)
            //            {
            //                item = new GroupedActionItemCollection();
            //                item.Key = todo.Priority;
            //                this.ViewSource.Add(item);
            //            }

            //            item.Add(todo);
            //        }

            //        break;

            //    case NotifyCollectionChangedAction.Remove:
            //    case NotifyCollectionChangedAction.Replace:
            //        break;

            //    case NotifyCollectionChangedAction.Reset:
            //        break;
            //}

            //var query = this.GroupBy(t => t.Priority);

            //foreach (var grouping in query)
            //{
            //    TodoGroupItem todoItemGroup = new TodoGroupItem();
            //    todoItemGroup.Key = grouping.Key;
            //    todoItemGroup.AddRange(grouping);
            //    //                groups.Add(todoItemGroup);
            //}

        }

        public static ActionItemCollection Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ActionItemCollection();
                }

                return instance;
            }
        }

        public ActionItem FindByValue(ActionItem actionItem)
        {
            return this.Where(a => a.Raw == actionItem.Raw).FirstOrDefault();
        }

        public bool ContainsValue(ActionItem actionItem)
        {
            return this.FindByValue(actionItem) != null;
        }
    }
}
