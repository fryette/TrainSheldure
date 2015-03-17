using System.Collections.Generic;
using System.Threading.Tasks;
using Trains.Entities;

namespace Trains.Services.Interfaces
{
    public interface ILastRequestTrainService
    {
        Task<List<Train>> GetTrains();
    }
}
