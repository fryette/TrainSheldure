using System.Collections.Generic;
using System.Threading.Tasks;
using TrainSearch.Entities;

namespace Trains.Services.Interfaces
{
    public interface ISearch
    {
        Task<IEnumerable<CountryStopPointDataItem>> GetCountryStopPoint();
        Task<IEnumerable<Train>> GetTrainSchedule(bool isEconom, bool specialSearch, params string[] parameters);
    }
}
