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
        public static async Task<T> ReadObjectFromXmlFileAsync<T>(string filename)where T : class
        {
            // this reads XML content from a file ("filename") and returns an object  from the XML
            var serializer = new XmlSerializer(typeof(T));
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            StorageFile file = await folder.GetFileAsync(filename);
            if (file == null) return null;
            Stream stream = await file.OpenStreamForReadAsync();
            var objectFromXml = (T)serializer.Deserialize(stream);
            stream.Dispose();
            return objectFromXml;
        }

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

        internal static async Task<IEnumerable<Train>> ReadObjectFromXmlFileAsync1(string filename)
        {
            var serializer = new XmlSerializer(typeof(List<Train>));
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            StorageFile file = await folder.GetFileAsync(filename);
            if (file == null) return null;
            Stream stream = await file.OpenStreamForReadAsync();
            var objectFromXml = (IEnumerable<Train>)serializer.Deserialize(stream);
            stream.Dispose();
            return objectFromXml;
        }
    }
}
