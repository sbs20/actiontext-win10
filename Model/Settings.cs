using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;

namespace Sbs20.Actiontext.Model
{
    class Settings
    {
        private const string SettingLocalStorageFile = "LocalStorageFile";
        private const string SettingIsLightTheme = "IsLightTheme";
        private const string SettingIsDeleteKeyActive = "IsDeleteKeyActive";
        private const string SettingPreservePriorityOnComplete = "PreservePriorityOnComplete";

        public static void ClearLocalFileReference()
        {
            if (StorageApplicationPermissions.FutureAccessList.ContainsItem(SettingLocalStorageFile))
            {
                StorageApplicationPermissions.FutureAccessList.Remove(SettingLocalStorageFile);
            }
        }

        public static async Task SelectLocalFileAsync()
        {
            var filePicker = new FileOpenPicker()
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };

            filePicker.FileTypeFilter.Add("*");

            if (true)
            {
                var file = await filePicker.PickSingleFileAsync();

                if (file != null)
                {
                    StorageApplicationPermissions.FutureAccessList.AddOrReplace(SettingLocalStorageFile, file);
                }
            }
        }

        public static async Task<StorageFile> GetLocalFileAsync()
        {
            if (StorageApplicationPermissions.FutureAccessList.ContainsItem(SettingLocalStorageFile))
            {
                try
                {
                    return await StorageApplicationPermissions.FutureAccessList.GetFileAsync(SettingLocalStorageFile);
                }
                catch { }
            }

            return null;
        }

        public static ApplicationTheme ApplicationTheme
        {
            get
            {
                if (ApplicationData.Current.LocalSettings.Values.ContainsKey(SettingIsLightTheme))
                {
                    return (bool)ApplicationData.Current.LocalSettings.Values[SettingIsLightTheme] ? ApplicationTheme.Light : ApplicationTheme.Dark;
                }

                return ApplicationTheme.Light;
            }
            set
            {
                ApplicationData.Current.LocalSettings.Values[SettingIsLightTheme] = value == ApplicationTheme.Light;
            }
        }

        private static object get(string key, object def)
        {
            return ApplicationData.Current.LocalSettings.Values[key] ?? def;
        }

        public static bool IsDeleteKeyActive
        {
            get { return Convert.ToBoolean(get(SettingIsDeleteKeyActive, false)); }
            set { ApplicationData.Current.LocalSettings.Values[SettingIsDeleteKeyActive] = value; }
        }

        public static bool PreservePriorityOnComplete
        {
            get { return Convert.ToBoolean(get(SettingPreservePriorityOnComplete, true)); }
            set { ApplicationData.Current.LocalSettings.Values[SettingPreservePriorityOnComplete] = value; }
        }
    }
}
