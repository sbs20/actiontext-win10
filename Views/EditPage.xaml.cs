using Sbs20.Actiontext.ViewModel;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace Sbs20.Actiontext.Views
{
    public sealed partial class EditPage : Page
    {
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


        private void OnBackRequested()
        {
            // Page above us will be our master view.
            // Make sure we are using the "drill out" animation in this transition.
            Frame.GoBack();
        }

        private void EditPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            // Mark event as handled so we don't get bounced out of the app.
            e.Handled = true;
            OnBackRequested();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            OnBackRequested();
        }

        private void BodyEdit_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                e.Handled = true;
                this.CopyTextBoxValueIntoObject();
                OnBackRequested();
            }
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
            }
        }
    }
}
