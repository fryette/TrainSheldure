using System.Collections.Generic;
using System.Threading.Tasks;
using Trains.Model.Entities;

namespace Trains.Services.Interfaces
{
    public interface ISerializableService
    {
        Task<bool> CheckIsFile(string fileName);
        List<LastRequest> SerializeLastRequest(string from, string to, List<LastRequest> lastrequsts);
        void SerializeObjectToXml<T>(T obj, string fileName);
        Task<List<LastRequest>> GetLastRequests(string fileName);
        void DeleteFile(string fileName);
        Task<T> ReadObjectFromXmlFileAsync<T>(string filename)where T:class ;
    }
}
