using System.Collections.Generic;
using System.Threading.Tasks;
using Trains.Model.Entities;

namespace Trains.Services.Interfaces
{
    public interface ITrainStopService
    {
        Task<IEnumerable<TrainStop>> GetTrainStop(string link);
    }
}
