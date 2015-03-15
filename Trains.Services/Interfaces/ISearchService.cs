using System.Collections.Generic;
using System.Threading.Tasks;
using Trains.Model.Entities;
using Trains.Entities;

namespace Trains.Services.Interfaces
{
    public interface ISearchService
    {
        Task<IEnumerable<CountryStopPointDataItem>> GetCountryStopPoint();
        Task<IEnumerable<Train>> GetTrainSchedule(params string[] parameters);
    }
}
