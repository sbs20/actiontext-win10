using System;
using Windows.Storage;
using Sbs20.Actiontext.Model;

namespace Sbs20.Actiontext.ViewModel
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
            var file = await FileStorageProvider.LoadFileAsync();
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
