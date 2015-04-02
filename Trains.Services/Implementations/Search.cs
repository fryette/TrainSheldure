using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trains.Infrastructure.Infrastructure;
using Trains.Model.Entities;
using Trains.Services.Interfaces;
using Trains.Entities;
using Trains.Services.Tools;

namespace Trains.Services.Implementations
{
    public class Search : ISearchService
    {
        public async Task<IEnumerable<CountryStopPointDataItem>> GetCountryStopPoint()
        {
            //return (await CountryStopPointData.GetGroupsAsync()).SelectMany(dataGroup => dataGroup.Items);
            return null;
        }
        public async Task<List<Train>> GetTrainSchedule(string from, string to, string date)
        {
            //return await TrainGrabber.GetTrainSchedule(from, to, date);
            return null;
        }

        public async Task<List<Train>> UpdateTrainSchedule()
        {
            //if (SavedItems.UpdatedLastRequest == null) return null;
            //return await TrainGrabber.GetTrainSchedule(SavedItems.UpdatedLastRequest.From,
            //                    SavedItems.UpdatedLastRequest.To, ToolHelper.GetDate(SavedItems.UpdatedLastRequest.Date, SavedItems.UpdatedLastRequest.SelectionMode));
            return null;
        }
    }
}
