using System.Collections.Generic;
using System.Threading.Tasks;
using Trains.Infrastructure.Infrastructure;
using Trains.Model.Entities;
using Trains.Services.Interfaces;
using Trains.Entities;

namespace Trains.Services.Implementations
{
    public class MainService:ILastRequestTrainService
    {
        public async Task<List<Train>> GetTrains()
        {
            return await Serialize.ReadObjectFromXmlFileAsync<List<Train>>(FileName.LastTrainList);
        }
    }
}
