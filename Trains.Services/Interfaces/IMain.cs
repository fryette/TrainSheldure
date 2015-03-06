using System.Collections.Generic;
using System.Threading.Tasks;
using TrainSearch.Entities;

namespace Trains.Services.Interfaces
{
    public interface IMain
    {
        Task<IEnumerable<Train>> GetTrains();
        Task<IEnumerable<MenuDataItem>> GetMenuData();

    }
}
