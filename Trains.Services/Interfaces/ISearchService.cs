using System.Collections.Generic;
using System.Threading.Tasks;
using Trains.Model.Entities;
using Trains.Entities;

namespace Trains.Services.Interfaces
{
    public interface ISearchService
    {
        Task<List<Train>> GetTrainSchedule(CountryStopPointItem from, CountryStopPointItem to, System.DateTimeOffset Datum, string SelectedVariant);
    }
}
