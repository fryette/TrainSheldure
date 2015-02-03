using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Windows.Storage;
using TrainShedule_HubVersion.Data;


namespace TrainShedule_HubVersion.DataModel
{
    class Serialize
    {
        public static async Task SaveObjectToXml<T>(T objectToSave, string filename)
        {
            // stores an object in XML format in file called 'filename'
            var serializer = new XmlSerializer(typeof(T));
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            StorageFile file = await folder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
            Stream stream = await file.OpenStreamForWriteAsync();
            using (stream)
            {
                serializer.Serialize(stream, objectToSave);
            }
        }

        internal static async Task<IEnumerable<T>> ReadObjectFromXmlFileAsync<T>(string filename)
        {
            var serializer = new XmlSerializer(typeof(List<T>));
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            if (!await CheckIsFile(filename)) return null;
            StorageFile file = await folder.GetFileAsync(filename);
            Stream stream = await file.OpenStreamForReadAsync();
            var objectFromXml = (IEnumerable<T>)serializer.Deserialize(stream);
            stream.Dispose();
            return objectFromXml;
        }

        public static async Task<bool> CheckIsFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return false;
            StorageFolder folder = ApplicationData.Current.LocalFolder;
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