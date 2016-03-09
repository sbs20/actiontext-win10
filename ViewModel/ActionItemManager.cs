using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Windows.Storage;
using Sbs20.Actiontext.Model;

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

            for (int index = 0; index < lines.Count; index++)
            {
                var line = lines[index];
                if (line.Trim().Length > 0)
                {
                    ActionItem actionItem = new ActionItem(line, index);

                    if (!Actions.ContainsValue(actionItem))
                    {
                        Actions.Add(actionItem);
                    }
                }
            }
        }

        public static async Task SaveAsync()
        {
            var file = await FileStorageProvider.LoadFileAsync();
            var lines = Actions.OrderBy(i => i.Index).Select(i => ActionItemAdapter.ToString(i));
            // await FileIO.WriteLinesAsync(file, lines);
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
            return new ActionItem(ActionItemAdapter.ToString(DateTime.Today, string.Empty), 0);
        }
    }
}
