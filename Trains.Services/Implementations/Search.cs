using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trains.Services.Interfaces;
using TrainSearch.Entities;
using TrainSearch.Infrastructure;

namespace Trains.Services.Implementations
{
    public class Search : ISearch
    {
        public async Task<IEnumerable<CountryStopPointDataItem>> GetCountryStopPoint()
        {
            return (await CountryStopPointData.GetGroupsAsync()).SelectMany(dataGroup => dataGroup.Items);
        }
        public async Task<IEnumerable<Train>> GetTrainSchedule(bool isEconom, bool specialSearch, params string[] parameters)
        {
            return await TrainGrabber.GetTrainSchedule(parameters[0].Split('(')[0], parameters[1].Split('(')[0], parameters[2], parameters[3],
                             isEconom, specialSearch);
        }
    }
}
