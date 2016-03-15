using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Sbs20.Actiontext.Model;
using Sbs20.Actiontext.ViewModel;
using Windows.UI.Popups;

namespace Sbs20.Actiontext.Views
{
    public sealed partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            this.InitializeComponent();
            this.DarkThemeToggle.IsOn = Settings.ApplicationTheme == ApplicationTheme.Dark;
            this.DeleteKeyToggle.IsOn = Settings.IsDeleteKeyActive;
            this.PreservePriorityOnCompleteToggle.IsOn = Settings.PreservePriorityOnComplete;
            this.ThemeInfoVisibilty();
        }

        private async void ChangeStorageLocation_Click(object sender, RoutedEventArgs e)
        {
            await Settings.SelectLocalFileAsync();
            await ActionItemManager.ReloadAsync();
        }

        private void ApplyTheme_Click(object sender, RoutedEventArgs e)
        {
            this.RequestedTheme = ElementTheme.Dark;
        }

        private void ClearStorageLocation_Click(object sender, RoutedEventArgs e)
        {
            Settings.ClearLocalFileReference();
            ActionItemManager.Actions.Clear();
        }

        private void ThemeInfoVisibilty()
        {
            this.ThemeInfo.Visibility = App.Current.RequestedTheme == Settings.ApplicationTheme ?
                Visibility.Collapsed :
                Visibility.Visible;
        }

        private void Toggle_Toggled(object sender, RoutedEventArgs e)
        {
            var toggle = sender as ToggleSwitch;

            switch (toggle.Name)
            {
                case "PreservePriorityOnCompleteToggle":
                    Settings.PreservePriorityOnComplete = toggle.IsOn;
                    break;

                case "DeleteKeyToggle":
                    Settings.IsDeleteKeyActive = this.DeleteKeyToggle.IsOn;
                    break;

                case "DarkThemeToggle":
                    Settings.ApplicationTheme = toggle.IsOn ? ApplicationTheme.Dark : ApplicationTheme.Light;
                    this.ThemeInfoVisibilty();
                    break;

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
