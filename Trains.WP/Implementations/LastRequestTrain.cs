using System.Collections.Generic;
using System.Threading.Tasks;
using Trains.Entities;
using Trains.Model.Entities;
using Trains.Services.Interfaces;
using Trains.WP.Infrastructure;

namespace Trains.WP.Implementations
{
    public class LastRequestTrain : ILastRequestTrainService
    {
        public async Task<List<Train>> GetTrains()
        {
            return await Serialize.ReadObjectFromXmlFileAsync<List<Train>>(FileName.LastTrainList);
        }
    }
}
