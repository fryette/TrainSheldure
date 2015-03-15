using System.Collections.Generic;
using System.Threading.Tasks;
using Trains.Infrastructure.Infrastructure;
using Trains.Services.Interfaces;
using Trains.Entities;

namespace Trains.Services.Implementations
{
    public class MainService:ILastRequestTrainService
    {
        public async Task<IEnumerable<Train>> GetTrains()
        {
            return await Serialize.ReadObjectFromXmlFileAsync<Train>("LastTrainList");
        }
    }
}
