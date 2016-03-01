using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using TodoTask = ToDoLib.Task;

namespace sbs20.Actiontext.Model
{
    public class ActionItemManager
    {
        public ActionItemCollection Items
        {
            get; private set;
        }

        public ActionItemManager()
        {
            this.Items = ActionItemCollection.Instance;
        }

        public async void Load()
        {
            var file = await StorageProvider.LoadFileAsync();
            var lines = await FileIO.ReadLinesAsync(file);
            foreach (string line in lines)
            {
                this.Items.Add(new TodoTask(line));
            }
        }

        private void Merge(ToDoLib.TaskList tasklist, IEnumerable<TodoTask> tasks)
        {

        }
    }
}
