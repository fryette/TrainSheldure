using System.Collections.Generic;
using System.Threading.Tasks;
using Trains.Services.Interfaces;
using TrainSearch.Infrastructure;

namespace Trains.Services.Implementations
{
    public class TrainStop:ITrainStop
    {
        public async Task<IEnumerable<TrainSearch.Entities.TrainStop>> GetTrainStop(string link)
        {
            return await Task.Run(()=>TrainStopGrabber.GetTrainStop(link));
        }
    }
}
