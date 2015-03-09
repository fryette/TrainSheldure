using System.Collections.Generic;
using System.Threading.Tasks;
using TrainSearch.Entities;

namespace Trains.Services.Interfaces
{
    public interface ITrainStopService
    {
        Task<IEnumerable<TrainStop>> GetTrainStop(string link);
    }
}
