using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Sbs20.Actiontext.ViewModel;

namespace Sbs20.Actiontext.Views
{
    public sealed partial class MainPage : Page
    {
        ActionItemManager manager;

        public MainPage()
        {
            this.InitializeComponent();
            this.manager = new ActionItemManager();

            //this.data.Source = TaskCollection.Instance.ViewSource;
            this.ActionItems.ItemsSource = ActionItemCollection.Instance;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            this.manager.Reload();

            VisualStateManager.GoToState(this, this.StandardState.Name, true);
        }

        private void AdaptiveStates_CurrentStateChanged(object sender, VisualStateChangedEventArgs e)
        {
        }

        private void ActionItems_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private async void Add_Click(object sender, RoutedEventArgs e)
        {
            //this.selectedNote = await NoteAdapter.CreateNoteAsync();
            //this.MasterListView.SelectedItem = this.selectedNote;
            //this.MasterListView.ScrollIntoView(this.selectedNote);
        }

        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            //IList<Note> toBeDeleted = new List<Note>();

            //// We have to make a temporary list of things to delete as doing so in the loop
            //// will invalidate the IEnumerable
            //foreach (Note note in this.MasterListView.SelectedItems)
            //{
            //    toBeDeleted.Add(note);
            //}

            //foreach (var note in toBeDeleted)
            //{
            //    await NoteAdapter.DeleteNoteAsync(note);
            //}
        }

        private void Multiselect_Click(object sender, RoutedEventArgs e)
        {
            if (this.ActionItems.Items.Count > 0)
            {
                VisualStateManager.GoToState(this, this.MultipleSelectionState.Name, true);
                this.ActionItems.SelectedItem = null;
            }
        }

        private void CancelMultiselect_Click(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, this.StandardState.Name, true);
        }

        private async void Refresh_Click(object sender, RoutedEventArgs e)
        {
            //await NoteAdapter.TryReadAllFromStorageAsync();
            //this.SelectMostAppropriateNote();
        }
    }
}
