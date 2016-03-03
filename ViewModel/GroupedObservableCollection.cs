using System.Collections.ObjectModel;

namespace Sbs20.Actiontext.ViewModel
{
    public class GroupedObservableCollection<T> : ObservableCollection<T>
    {
        public string Key { get; set; }
    }
}
