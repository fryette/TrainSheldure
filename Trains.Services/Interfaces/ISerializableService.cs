using System.Collections.Generic;
using System.Threading.Tasks;
using Trains.Model.Entities;

namespace Trains.Services.Interfaces
{
    public interface ISerializableService
    {
        Task<bool> CheckIsFile(string fileName);
        Task SerializeObjectToXml<T>(T obj, string fileName);
        Task DeleteFile(string fileName);
        Task<T> ReadObjectFromXmlFileAsync<T>(string filename) where T : class;
    }
}
