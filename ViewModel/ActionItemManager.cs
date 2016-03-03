using System;
using Windows.Storage;
using Sbs20.Actiontext.Model;
using System.Threading.Tasks;

namespace Sbs20.Actiontext.ViewModel
{
    public class ActionItemManager
    {
        public ActionItem Selected { get; set; }
        public ActionItemCollection Actions { get; private set; }

        public ActionItemManager()
        {
            this.Actions = ActionItemCollection.Instance;
        }

        public async Task Reload()
        {
            var file = await FileStorageProvider.LoadFileAsync();
            var lines = await FileIO.ReadLinesAsync(file);
            foreach (string line in lines)
            {
                ActionItem actionItem = ActionItem.Parse(line);

                if (!this.Actions.ContainsValue(actionItem))
                {
                    this.Actions.Add(actionItem);
                }
            }
        }

        public async Task Delete(ActionItem actionItem)
        {
            if (this.Actions.Contains(actionItem))
            {
                this.Actions.Remove(actionItem);
            }
        }
    }
}
