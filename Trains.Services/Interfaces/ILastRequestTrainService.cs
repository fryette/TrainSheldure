using System.Collections.Generic;
using System.Threading.Tasks;
using TrainSearch.Entities;

namespace Trains.Services.Interfaces
{
    public interface ILastRequestTrainService
    {
        Task<IEnumerable<Train>> GetTrains();
    }
}
