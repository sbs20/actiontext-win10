using System.Collections.Generic;
using System.Linq;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using Sbs20.Actiontext.Extensions;
using Sbs20.Actiontext.ViewModel;
using Sbs20.Actiontext.Model;
using System.Threading.Tasks;

namespace Sbs20.Actiontext.Views
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.ActionItems.ItemsSource = ActionItemManager.Actions;
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            bool hasFile = await Settings.GetLocalFileAsync() != null;

            if (hasFile)
            {
                this.Message.Visibility = Visibility.Collapsed;
                ActionItemManager.Actions.Sort();
                this.SelectActionItemAndScroll();
                VisualStateManager.GoToState(this, this.StandardState.Name, true);
            }
            else
            {
                this.Message.Visibility = Visibility.Visible;
                VisualStateManager.GoToState(this, this.NoFileState.Name, true);
            }
        }

        private void SelectActionItemAndScroll()
        {
            if (ActionItemManager.Actions.Count > 0)
            {
                if (ActionItemManager.Selected != null && !this.ActionItems.Items.Contains(ActionItemManager.Selected))
                {
                    ActionItemManager.Selected = ActionItemManager.Actions.FindByValue(ActionItemManager.Selected);
                }

                if (ActionItemManager.Selected == null)
                {
                    ActionItemManager.Selected = ActionItemManager.Actions[0];
                }

                this.ActionItems.SelectedItem = ActionItemManager.Selected;
            }

            this.ActionItems.ScrollIntoView(ActionItemManager.Selected);
        }

        private void AdaptiveStates_CurrentStateChanged(object sender, VisualStateChangedEventArgs e)
        {
        }

        private void ActionItems_ItemClick(object sender, ItemClickEventArgs e)
        {
            ActionItemManager.Selected = e.ClickedItem as ActionItem;
        }

        private void ActionItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                ActionItemManager.Selected = e.AddedItems[0] as ActionItem;
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            this.Create();
        }

        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            await this.DeleteAsync();
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
            await ActionItemManager.ReloadAsync();
            this.SelectActionItemAndScroll();
        }

        private void Create()
        {
            ActionItemManager.Selected = ActionItemManager.Create();
            ActionItemManager.Actions.Add(ActionItemManager.Selected);
            this.ActionItems.SelectedItem = ActionItemManager.Selected;
            this.Edit();
        }

        private void Edit()
        {
            ActionItemManager.Selected = this.ActionItems.SelectedItem as ActionItem;
            if (ActionItemManager.Selected != null)
            {
                Frame.Navigate(typeof(EditPage), ActionItemManager.Selected);
            }
        }

        private async Task DeleteAsync()
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
                ActionItemManager.Delete(actionItem);
            }

            await ActionItemManager.SaveAsync();
        }

        private async void ActionItems_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            switch (e.Key)
            {
                case VirtualKey.F2:
                case VirtualKey.U:
                    this.Edit();
                    break;

                case VirtualKey.N:
                    this.Create();
                    break;

                case VirtualKey.X:
                    this.SelectedIsComplete_Toggle();
                    break;

                case VirtualKey.Delete:
                    if (Settings.IsDeleteKeyActive)
                    {
                        await this.DeleteAsync();
                    }
                    break;
            }
        }

        private async void SelectedIsComplete_Toggle()
        {
            // Only do anything if something is selected otherwise badness will happen
            if (ActionItemManager.Selected != null)
            {
                // We do need to worry about setting data here since this is non-standard 
                ActionItemManager.Selected.IsComplete = !ActionItemManager.Selected.IsComplete;

                // And save
                await ActionItemManager.SaveAsync();

                // Now sort
                ActionItemManager.Actions.Sort();
                this.SelectActionItemAndScroll();
            }
        }

        private async void IsComplete_Click(object sender, RoutedEventArgs e)
        {
            // There is two way data binding going on - so what we don't need to do is worry
            // about storing the data. This is purely about refreshing the view on the basis
            // of changes

            // The thing being clicked isn't necessarily selected. So we need to fix that
            CheckBox checkbox = e.OriginalSource as CheckBox;
            ListViewItem container = checkbox.AllAncestry().First(el => el is ListViewItem) as ListViewItem;

            // It is really important we mark the container as selected. If we don't then we 
            // get crashes on sort. 
            container.IsSelected = true;

            // Similarly we need to make a note of the selected action
            ActionItemManager.Selected = this.ActionItems.ItemFromContainer(container) as ActionItem;

            // Now sort and select
            ActionItemManager.Actions.Sort();
            this.SelectActionItemAndScroll();

            // And save
            await ActionItemManager.SaveAsync();
        }

        private void ActionItems_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {

        }
    }
}
