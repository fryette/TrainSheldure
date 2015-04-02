using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Trains.Infrastructure.Infrastructure;
using Trains.Model.Entities;
using Trains.Services.Interfaces;
using Trains.Services.Tools;

namespace Trains.Services.Implementations
{
    public class TrainStop : ITrainStopService
    {
        public async Task<IEnumerable<Model.Entities.TrainStop>> GetTrainStop(string link)
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                return await Task.Run(() => TrainStopGrabber.GetTrainStop(link));
            }
            //ToolHelper.ShowMessageBox(SavedItems.ResourceLoader.GetString("InternetConnectionError"));
            return null;
        }
    }
}
