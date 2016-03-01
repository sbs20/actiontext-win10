using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace sbs20.Tasktxt.Model
{
    public class StorageProvider
    {
        public static async Task<StorageFile> LoadFileAsync()
        {
            return await Settings.GetStorageFileAsync();
        }

        public static async Task SaveFileAsync(string name, string data)
        {
            var file = await Settings.GetStorageFileAsync();

            try
            {
                await FileIO.WriteTextAsync(file, data);
            }
            catch (System.IO.FileNotFoundException)
            {
                throw;
            }
        }
    }
}