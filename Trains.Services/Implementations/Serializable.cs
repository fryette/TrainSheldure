using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trains.Infrastructure.Infrastructure;
using Trains.Model.Entities;
using Trains.Resources;
using Trains.Services.Interfaces;

namespace Trains.Services.Implementations
{
    public class Serializable
    {

        public Task<bool> CheckIsFile(string Constants)
        {
            return Serialize.CheckIsFile(Constants);
        }

        public List<LastRequest> SerializeLastRequest(string from, string to, List<LastRequest> lastRequests)
        {
            if (lastRequests == null) lastRequests = new List<LastRequest>(3);
            if (lastRequests.Any(x => x.From == from && x.To == to)) return lastRequests;
            lastRequests[2] = lastRequests[1];
            lastRequests[1] = lastRequests[0];
            lastRequests[0] = new LastRequest { From = from, To = to };
            
            SerializeObjectToXml(lastRequests, Constants.LastRequests);
            return lastRequests;
        }

        public async void SerializeObjectToXml<T>(T obj, string Constants)
        {
            await Serialize.SaveObjectToXml(obj, Constants);
        }

        public async Task<List<LastRequest>> GetLastRequests(string Constants)
        {
            var obj = await Serialize.ReadObjectFromXmlFileAsync<List<LastRequest>>(Constants);
            return obj == null ? null : obj.ToList();
        }

        public void DeleteFile(string Constants)
        {
            Serialize.DeleteFile(Constants);
        }

        public Task<T> ReadObjectFromXmlFileAsync<T>(string Constants) where T : class
        {
            return Serialize.ReadObjectFromXmlFileAsync<T>(Constants);
        }
    }
}
