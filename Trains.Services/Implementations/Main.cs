using System.Collections.Generic;
using System.Threading.Tasks;
using Trains.Services.Interfaces;
using TrainSearch.Entities;
using TrainSearch.Infrastructure;

namespace Trains.Services.Implementations
{
    public class MainService:IMain
    {
        public async Task<IEnumerable<Train>> GetTrains()
        {
            return await Serialize.ReadObjectFromXmlFileAsync<Train>("LastTrainList");
        }

        public async Task<IEnumerable<MenuDataItem>> GetMenuData()
        {
            return await MenuData.GetItemsAsync(); 
        }
    }
}
