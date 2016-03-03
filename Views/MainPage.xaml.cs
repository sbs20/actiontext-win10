using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Sbs20.Actiontext.ViewModel;
using System.Collections.Generic;

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

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            await this.manager.Reload();
            this.SelectMostAppropriateActionItem();
            VisualStateManager.GoToState(this, this.StandardState.Name, true);
        }

        private void SelectMostAppropriateActionItem()
        {
            if (this.manager.Actions.Count > 0)
            {
                if (this.manager.Selected != null && !this.ActionItems.Items.Contains(this.manager.Selected))
                {
                    // If we're navigating back to this page then we reload all the notes from disk in which
                    // case we will have new references.
                    this.manager.Selected = this.manager.Actions.FindByValue(this.manager.Selected);
                }

                if (this.manager.Selected == null)
                {
                    this.manager.Selected = this.manager.Actions[0];
                }

                this.ActionItems.SelectedItem = this.manager.Selected;
            }
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
            IList<ActionItem> toBeDeleted = new List<ActionItem>();

            // We have to make a temporary list of things to delete as doing so in the loop
            // will invalidate the IEnumerable
            foreach (ActionItem actionItem in this.ActionItems.SelectedItems)
            {
                toBeDeleted.Add(actionItem);
            }

            foreach (var actionItem in toBeDeleted)
            {
                await this.manager.Delete(actionItem);
            }
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
            await this.manager.Reload();
            this.SelectMostAppropriateActionItem();
        }
    }
}
