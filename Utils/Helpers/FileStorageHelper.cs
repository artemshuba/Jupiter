using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Jupiter.Utils.Extensions;

namespace Jupiter.Utils.Helpers
{
    public static class FileStorageHelper
    {
        public static async Task<bool> IsFileExists(string path, IStorageFolder rootFolder = null, bool phone = false)
        {
            var localFolder = rootFolder ?? ApplicationData.Current.LocalFolder;
            StorageFile file = null;
            if (!phone)
            {
                file = await localFolder.TryGetItemAsync(path) as StorageFile;
            }
            else
            {
                try
                {
                    file = await localFolder.GetFileAsync(path);
                }
                catch (Exception)
                {
                }
            }

            if (file != null)
                return true;

            return false;
        }

        public static async Task<bool> IsFolderExists(string path, IStorageFolder rootFolder = null, bool phone = false)
        {
            var localFolder = rootFolder ?? ApplicationData.Current.LocalFolder;
            StorageFolder folder = null;

            if (!phone)
            {
                folder = await localFolder.TryGetItemAsync(path) as StorageFolder;
            }
            else
            {
                try
                {
                    folder = await localFolder.GetFolderAsync(path);
                }
                catch (Exception)
                {
                }
            }

            if (folder != null)
                return true;

            return false;
        }


        public static async Task<StorageFolder> CreateFolder(string path, IStorageFolder rootFolder = null, CreationCollisionOption options = CreationCollisionOption.FailIfExists)
        {
            var localFolder = rootFolder ?? ApplicationData.Current.LocalFolder;
            return await localFolder.CreateFolderAsync(path, options);
        }

        public static async Task WriteText(string path, string text, IStorageFolder rootFolder = null)
        {
            var folder = rootFolder ?? ApplicationData.Current.LocalFolder;

            var file = await folder.CreateFileAsync(path, CreationCollisionOption.ReplaceExisting);

            using (var stream = await file.OpenStreamForWriteAsync())
            {
                stream.SetLength(0);
                stream.WriteText(text);
            }
        }

        public static async Task AppendText(string path, string text, IStorageFolder rootFolder = null)
        {
            var folder = rootFolder ?? ApplicationData.Current.LocalFolder;

            var file = await folder.CreateFileAsync(path, CreationCollisionOption.OpenIfExists);

            using (var stream = await file.OpenStreamForWriteAsync())
            {
                stream.Seek(stream.Length, SeekOrigin.Begin);
                stream.WriteText(text);
            }
        }

        public static async Task<string> ReadText(string path, IStorageFolder rootFolder = null)
        {
            var folder = rootFolder ?? ApplicationData.Current.LocalFolder;
            var file = await folder.TryGetItemAsync(path) as StorageFile;

            if (file == null)
                return null;

            using (var stream = await file.OpenStreamForReadAsync())
            {
                return stream.ReadText();
            }
        }

        public static async Task<Stream> OpenFileRead(string path, IStorageFolder rootFolder = null)
        {
            var folder = rootFolder ?? ApplicationData.Current.LocalFolder;
            var file = await folder.GetFileAsync(path);

            return (await file.OpenReadAsync()).AsStreamForRead();
        }

        public static async Task<Stream> OpenFileWrite(string path, IStorageFolder rootFolder = null)
        {
            var folder = rootFolder ?? ApplicationData.Current.LocalFolder;
            var file = await folder.CreateFileAsync(path, CreationCollisionOption.ReplaceExisting);

            return (await file.OpenAsync(FileAccessMode.ReadWrite)).AsStreamForWrite();
        }

        public static string GetSafeFileName(string input)
        {
            var fileName = input;
            foreach (var c in Path.GetInvalidFileNameChars())
            {
                fileName = fileName.Replace(c, '-');
            }

            return fileName;
        }

        public static async Task DeleteFile(string path, IStorageFolder rootFolder = null)
        {
            var folder = rootFolder ?? ApplicationData.Current.LocalFolder;
            var file = await folder.GetItemAsync(path);
            await file.DeleteAsync(StorageDeleteOption.PermanentDelete);
        }
    }
}
