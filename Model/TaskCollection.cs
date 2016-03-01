using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoItem = ToDoLib.Task;

namespace sbs20.Tasktxt.Model
{
    public class TaskCollection : SortedObservableCollection<TodoItem>
    {
        // Singleton
        private static TaskCollection instance;

        // Members
        public ObservableCollection<TodoGroupItem> ViewSource { get; private set; }

        private TaskCollection()
        {
            this.ViewSource = new ObservableCollection<TodoGroupItem>();
            this.SortKey = i =>
            {
                try
                {
                    string s = !i.Completed ? i.CreationDate : i.CompletedDate;
                    DateTime creationDate = DateTime.Parse(s);
                    TimeSpan span = DateTime.MaxValue - creationDate;
                    return (i.Completed ? "1" : "0") + i.Priority + ":" + span.Ticks + ":" + i.Body;
                }
                catch { }
                return "";
            };

            this.CollectionChanged += TaskCollection_CollectionChanged;
        }

        private void TaskCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (TodoItem todo in e.NewItems)
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
                    int bbb = 0;
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

        public static TaskCollection Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TaskCollection();
                }

                return instance;
            }
        }
    }
}
