using System;
using System.Collections.Generic;
using Windows.Storage;

namespace sbs20.Actiontext.Model
{
    public class ActionItemManager
    {
        public ActionItemCollection Actions
        {
            get; private set;
        }

        public ActionItemManager()
        {
            this.Actions = ActionItemCollection.Instance;
        }

        public async void Load()
        {
            var file = await StorageProvider.LoadFileAsync();
            var lines = await FileIO.ReadLinesAsync(file);
            foreach (string line in lines)
            {
                this.Actions.Add(ActionItem.Parse(line));
            }
        }

        //private void Merge(ToDoLib.TaskList tasklist, IEnumerable<TodoTask> tasks)
        //{

        //}
    }
}
