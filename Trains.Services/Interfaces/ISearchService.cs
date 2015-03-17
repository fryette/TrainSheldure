using System.Collections.Generic;
using System.Threading.Tasks;
using Trains.Model.Entities;
using Trains.Entities;

namespace Trains.Services.Interfaces
{
    public interface ISearchService
    {
        Task<IEnumerable<CountryStopPointDataItem>> GetCountryStopPoint();
        Task<List<Train>> GetTrainSchedule(string from, string to, string date);
    }
}
