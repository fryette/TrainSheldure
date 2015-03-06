using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trains.Services.Interfaces;
using TrainSearch.Entities;
using TrainSearch.Infrastructure;

namespace Trains.Services.Implementations
{
    public class Serializable:ISerializable
    {
        public void SerializeLastRequest(string from, string to, List<LastRequest> lastRequests)
        {
            if (lastRequests == null) lastRequests = new List<LastRequest>();
            if (lastRequests.Any(x => x.From == from && x.To == to)) return;
            if (lastRequests.Count < 3)
            {
                lastRequests.Add(new LastRequest
                {
                    From = from,
                    To = to
                });
            }
            else
            {
                lastRequests[2] = lastRequests[1];
                lastRequests[1] = lastRequests[0];
                lastRequests[0] = new LastRequest
                {
                    From = from,
                    To = to
                };
            }
            SerializeObjectToXml(lastRequests, "lastRequests");
        }

        public async void SerializeObjectToXml<T>(T obj, string fileName)
        {
            await Serialize.SaveObjectToXml(obj, fileName);
        }

        public async Task<List<LastRequest>> GetLastRequests(string fileName)
        {
            var obj = await Serialize.ReadObjectFromXmlFileAsync<LastRequest>(fileName);
            return obj == null ? null : obj.ToList();
        }

        public void DeleteFile(string fileName)
        {
            Serialize.DeleteFile("favoriteRequests");
        }
    }
}
