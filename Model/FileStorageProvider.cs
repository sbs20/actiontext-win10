using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace Sbs20.Actiontext.Model
{
    public class FileStorageProvider
    {
        public static async Task<StorageFile> GetFileAsync()
        {
            return await Settings.GetLocalFileAsync();
        }

        public static async Task SaveFileAsync(string name, string data)
        {
            var file = await Settings.GetLocalFileAsync();

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