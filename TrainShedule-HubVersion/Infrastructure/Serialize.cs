using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.Storage;

namespace TrainShedule_HubVersion.Infrastructure
{
    internal class Serialize
    {
        public static async Task SaveObjectToXml<T>(T objectToSave, string filename)
        {
            var serializer = new XmlSerializer(typeof (T));
            var folder = ApplicationData.Current.LocalFolder;
            var file = await folder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
            var stream = await file.OpenStreamForWriteAsync();
            using (stream)
                serializer.Serialize(stream, objectToSave);
        }

        internal static async Task<IEnumerable<T>> ReadObjectFromXmlFileAsync<T>(string filename)
        {
            var serializer = new XmlSerializer(typeof (List<T>));
            var folder = ApplicationData.Current.LocalFolder;
            if (!await CheckIsFile(filename)) return null;
            var file = await folder.GetFileAsync(filename);
            var stream = await file.OpenStreamForReadAsync();
            var objectFromXml = (IEnumerable<T>) serializer.Deserialize(stream);
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
    }
}