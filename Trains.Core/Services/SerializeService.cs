using Cirrious.CrossCore;
using Cirrious.MvvmCross.Plugins.File;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trains.Core.Interfaces;

namespace Trains.Core.Services
{
    class SerializeService : ISerializableService
    {
        IMvxFileStore _fileStore;
        public SerializeService(IMvxFileStore fileStore)
        {
            _fileStore = fileStore;
        }

        public bool Exists(string fileName)
        {
            return _fileStore.Exists(fileName);
        }

        public void Serialize<T>(T obj, string fileName)
        {
            _fileStore.WriteFile(fileName, JsonConvert.SerializeObject(obj));
        }

        public void Delete(string fileName)
        {
            if (Exists(fileName))
                _fileStore.DeleteFile(fileName);
        }

        public T Desserialize<T>(string filename) where T : class
        {
            string textJson;
            _fileStore.TryReadTextFile(filename, out textJson);
            if (textJson == null) return null;
            return JsonConvert.DeserializeObject<T>(textJson);
        }
    }
}
