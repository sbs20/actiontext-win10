﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoItem = ToDoLib.Task;

namespace sbs20.Actiontext.Model
{
    public class ActionItemCollection : SortedObservableCollection<ActionItem>
    {
        // Singleton
        private static ActionItemCollection instance;

        // Members
        public ObservableCollection<TodoGroupItem> ViewSource { get; private set; }

        private ActionItemCollection()
        {
            this.ViewSource = new ObservableCollection<TodoGroupItem>();
            this.SortKey = i =>
            {
                TimeSpan span = DateTime.MaxValue - i.DisplayDate;
                return (i.IsComplete ? "1" : "0") + ":" + i.Priority + ":" + span.Ticks + ":" + i.Body;
            };

            this.CollectionChanged += TaskCollection_CollectionChanged;
        }

        private void TaskCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (ActionItem todo in e.NewItems)
                    {
                        var item = this.ViewSource.Where(i => i.Key == todo.Priority).FirstOrDefault();
                        if (item == null)
                        {
                            item = new TodoGroupItem();
                            item.Key = todo.Priority;
                            this.ViewSource.Add(item);
                        }

                        item.Add(todo);
                    }

                    break;

                case NotifyCollectionChangedAction.Remove:
                case NotifyCollectionChangedAction.Replace:
                    break;

                case NotifyCollectionChangedAction.Reset:
                    break;
            }

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
    }
}