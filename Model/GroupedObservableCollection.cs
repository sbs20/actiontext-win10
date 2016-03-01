using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace sbs20.Actiontext.Model
{
    public class GroupedObservableCollection<T> : ObservableCollection<T>
    {
        public string Key { get; set; }
    }
}
