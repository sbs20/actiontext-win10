using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Sbs20.Actiontext.ViewModel;
using Sbs20.Actiontext.Extensions;
using Windows.UI.Xaml.Input;
using Windows.System;

namespace Sbs20.Actiontext.Views
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            //this.data.Source = TaskCollection.Instance.ViewSource;
            this.ActionItems.ItemsSource = ActionItemManager.Actions;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ActionItemManager.Actions.Sort();
            this.SelectMostAppropriateActionItem();
            VisualStateManager.GoToState(this, this.StandardState.Name, true);
        }

        private void SelectMostAppropriateActionItem()
        {
            if (ActionItemManager.Actions.Count > 0)
            {
                if (ActionItemManager.Selected != null && !this.ActionItems.Items.Contains(ActionItemManager.Selected))
                {
                    // If we're navigating back to this page then we reload all the notes from disk in which
                    // case we will have new references.
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
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            ActionItemManager.Selected = ActionItemManager.Create();
            ActionItemManager.Actions.Add(ActionItemManager.Selected);
            this.ActionItems.SelectedItem = ActionItemManager.Selected;
            this.Edit();
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
                ActionItemManager.Delete(actionItem);
            }

            await ActionItemManager.SaveAsync();
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
            this.SelectMostAppropriateActionItem();
        }

        private void Edit()
        {
            ActionItemManager.Selected = this.ActionItems.SelectedItem as ActionItem;
            if (ActionItemManager.Selected != null)
            {
                Frame.Navigate(typeof(EditPage), ActionItemManager.Selected);
            }
        }

        private void ActionItems_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            switch (e.Key)
            {
                case VirtualKey.F2:
                    this.Edit();
                    break;
            }
        }
    }
}
