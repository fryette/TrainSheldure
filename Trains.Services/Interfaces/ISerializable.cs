using System.Collections.Generic;
using System.Threading.Tasks;
using TrainSearch.Entities;

namespace Trains.Services.Interfaces
{
    public interface ISerializable
    {
        void SerializeLastRequest(string from, string to, List<LastRequest> lastrequsts);
        void SerializeObjectToXml<T>(T obj, string fileName);
        Task<List<LastRequest>> GetLastRequests(string fileName);
        void DeleteFile(string fileName);
    }
}
