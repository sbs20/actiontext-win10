using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Sbs20.Actiontext.Model
{
    public class GroupedObservableCollection<T> : ObservableCollection<T>
    {
        public string Key { get; set; }
    }
}
