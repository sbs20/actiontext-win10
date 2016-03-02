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
        private const string LocalStorageFile = "LocalStorageFile";

        public static async Task SelectLocalFileAsync()
        {
            var filePicker = new FileOpenPicker()
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };

            filePicker.FileTypeFilter.Add("*");

            while (true)
            {
                var file = await filePicker.PickSingleFileAsync();

                if (file != null)
                {
                    StorageApplicationPermissions.FutureAccessList.AddOrReplace(LocalStorageFile, file);
                }

                if (StorageApplicationPermissions.FutureAccessList.ContainsItem(LocalStorageFile))
                {
                    break;
                }
            }
        }

        public static async Task<StorageFile> GetStorageFileAsync()
        {
            if (!StorageApplicationPermissions.FutureAccessList.ContainsItem(LocalStorageFile))
            {
                await SelectLocalFileAsync();
            }

            return await StorageApplicationPermissions.FutureAccessList.GetFileAsync(LocalStorageFile);
        }

        public static ApplicationTheme ApplicationTheme
        {
            get
            {
                if (ApplicationData.Current.LocalSettings.Values.ContainsKey("IsLightTheme"))
                {
                    return (bool)ApplicationData.Current.LocalSettings.Values["IsLightTheme"] ? ApplicationTheme.Light : ApplicationTheme.Dark;
                }

                return ApplicationTheme.Light;
            }
            set
            {
                ApplicationData.Current.LocalSettings.Values["IsLightTheme"] = value == ApplicationTheme.Light;
            }
        }
    }
}
