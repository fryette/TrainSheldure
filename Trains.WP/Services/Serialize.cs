using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Trains.Infrastructure.Interfaces;
using Trains.Services.Interfaces;
using Windows.Storage;

namespace Trains.WP.Services
{
    public class Serialize : ISerializableService
    {
        public async Task SerializeObjectToXml<T>(T objectToSave, string filename)
        {
            var serializer = new XmlSerializer(typeof(T));
            var folder = ApplicationData.Current.LocalFolder;
            var file = await folder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
            var stream = await file.OpenStreamForWriteAsync();
            using (stream)
                serializer.Serialize(stream, objectToSave);
        }

        public async Task<T> ReadObjectFromXmlFileAsync<T>(string filename) where T : class
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

        public async Task<bool> CheckIsFile(string fileName)
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
        public async Task DeleteFile(string fileName)
        {
            if (!(await CheckIsFile(fileName))) return;
            await (await ApplicationData.Current.LocalFolder.GetFileAsync(fileName)).DeleteAsync();
        }
    }
}