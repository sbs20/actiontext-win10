using Sbs20.Actiontext.ViewModel;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace Sbs20.Actiontext.Views
{
    public sealed partial class EditPage : Page
    {
        private string initialRaw;

        private static DependencyProperty s_actionItemProperty = DependencyProperty.Register("ActionItem",
            typeof(ActionItem),
            typeof(EditPage),
            new PropertyMetadata(null));

        public static DependencyProperty ActionItemProperty
        {
            get { return s_actionItemProperty; }
        }

        public ActionItem ActionItem
        {
            get { return (ActionItem)GetValue(s_actionItemProperty); }
            set { SetValue(s_actionItemProperty, value); }
        }

        public EditPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            this.ActionItem = e.Parameter as ActionItem;
            this.ActionItem.Restring();
            this.initialRaw = this.ActionItem.Raw;

            // Register for hardware and software back request from the system
            SystemNavigationManager.GetForCurrentView().BackRequested += EditPage_BackRequested;
        }

        protected override async void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (string.IsNullOrEmpty(this.ActionItem.Raw))
            {
                ActionItemManager.Delete(this.ActionItem);
            }
            else
            {
                // Try and save first...
                await ActionItemManager.SaveAsync();
            }

            // As you were
            base.OnNavigatedFrom(e);
            SystemNavigationManager.GetForCurrentView().BackRequested -= EditPage_BackRequested;
        }

        /// <summary>
        /// x:Bind does not support UpdateSourceTrigger so we might have to force a "bind" to occur
        /// It's a bit miserable but at least it works
        /// </summary>
        private void CopyTextBoxValueIntoObject()
        {
            if (this.ActionItem != null)
            {
                this.ActionItem.Raw = this.RawEdit.Text;
                this.ActionItem.Reparse();
            }
        }

        private void GoBack()
        {
            Frame.GoBack();
        }

        private void AcceptChangesAndGoBack()
        {
            this.CopyTextBoxValueIntoObject();
            this.GoBack();
        }

        private void EditPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            // Mark event as handled so we don't get bounced out of the app.
            e.Handled = true;
            this.AcceptChangesAndGoBack();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.AcceptChangesAndGoBack();
        }

        private void BodyEdit_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            switch (e.Key)
            {
                case VirtualKey.Enter:
                    e.Handled = true;
                    this.AcceptChangesAndGoBack();
                    break;

                case VirtualKey.Escape:
                    e.Handled = true;
                    this.GoBack();
                    break;
            }
        }
    }
}
