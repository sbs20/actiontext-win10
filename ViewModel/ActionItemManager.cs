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

        public async void Reload()
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
    }
}
