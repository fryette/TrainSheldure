using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.Storage;

namespace Trains.Infrastructure.Infrastructure
{
    public class Serialize
    {
        public static async Task SaveObjectToXml<T>(T objectToSave, string filename)
        {
            var serializer = new XmlSerializer(typeof(T));
            var folder = ApplicationData.Current.LocalFolder;
            var file = await folder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
            var stream = await file.OpenStreamForWriteAsync();
            using (stream)
                serializer.Serialize(stream, objectToSave);
        }

        public static async Task<T> ReadObjectFromXmlFileAsync<T>(string filename) where T : class
        {
            var serializer = new XmlSerializer(typeof(T));
            var folder = ApplicationData.Current.LocalFolder;
            if (!await CheckIsFile(filename)) return null;
            var file = await folder.GetFileAsync(filename);
            var stream = await file.OpenStreamForReadAsync();
            var objectFromXml = (T)serializer.Deserialize(stream);
            stream.Dispose();
            return objectFromXml;
        }

        public static async Task<bool> CheckIsFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return false;
            var folder = ApplicationData.Current.LocalFolder;
            try
            {
                await folder.GetFileAsync(fileName);
                return true; //exist
            }
            catch
            {
                return false; // not exist
            }
        }
        public static async void DeleteFile(string fileName)
        {
            var filed = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
            if (filed != null)
            {
                await filed.DeleteAsync();
            }
        }
    }
}