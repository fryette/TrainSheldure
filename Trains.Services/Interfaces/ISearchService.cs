using System.Collections.Generic;
using System.Threading.Tasks;
using TrainSearch.Entities;

namespace Trains.Services.Interfaces
{
    public interface ISearchService
    {
        Task<IEnumerable<CountryStopPointDataItem>> GetCountryStopPoint();
        Task<IEnumerable<Train>> GetTrainSchedule(params string[] parameters);
    }
}
