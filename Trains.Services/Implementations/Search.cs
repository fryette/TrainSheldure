using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trains.Infrastructure.Infrastructure;
using Trains.Model.Entities;
using Trains.Services.Interfaces;
using Trains.Entities;

namespace Trains.Services.Implementations
{
    public class Search : ISearchService
    {
        public async Task<IEnumerable<CountryStopPointDataItem>> GetCountryStopPoint()
        {
            return (await CountryStopPointData.GetGroupsAsync()).SelectMany(dataGroup => dataGroup.Items);
        }
        public async Task<List<Train>> GetTrainSchedule(string from,string to,string date)
        {
            return await TrainGrabber.GetTrainSchedule(from, to, date);
        }
    }
}
