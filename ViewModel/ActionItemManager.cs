using System;
using Windows.Storage;
using Sbs20.Actiontext.Model;
using System.Threading.Tasks;

namespace Sbs20.Actiontext.ViewModel
{
    public static class ActionItemManager
    {
        public static ActionItem Selected { get; set; }
        public static ActionItemCollection Actions { get; private set; }

        static ActionItemManager()
        {
            Actions = ActionItemCollection.Instance;
        }

        public static async Task ReloadAsync()
        {
            var file = await FileStorageProvider.LoadFileAsync();
            var lines = await FileIO.ReadLinesAsync(file);
            foreach (string line in lines)
            {
                ActionItem actionItem = new ActionItem(line);

                if (!Actions.ContainsValue(actionItem))
                {
                    Actions.Add(actionItem);
                }
            }
        }

        public static async Task SaveAsync()
        {
            // TODO
        }

        public static void Delete(ActionItem actionItem)
        {
            if (Actions.Contains(actionItem))
            {
                Actions.Remove(actionItem);
            }
        }

        public static ActionItem Create()
        {
            return new ActionItem(ActionItem.ToRawString(DateTime.Today, string.Empty));
        }
    }
}
