using System.Collections.Generic;
using System.Threading.Tasks;
using Trains.Infrastructure.Infrastructure;
using Trains.Services.Interfaces;

namespace Trains.Services.Implementations
{
    public class TrainStop:ITrainStopService
    {
        public async Task<IEnumerable<TrainSearch.Entities.TrainStop>> GetTrainStop(string link)
        {
            return await Task.Run(()=>TrainStopGrabber.GetTrainStop(link));
        }
    }
}
