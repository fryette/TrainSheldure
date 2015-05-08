using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Trains.Core.Interfaces;
using Trains.Core.Resources;
using Trains.Core.Services.Infrastructure;
using Trains.Core.Services.Interfaces;
using Trains.Model.Entities;

namespace Trains.Core.Services
{
    public class TrainStopService : ITrainStopService
    {
        public readonly IHttpService HttpService;
        public readonly IPattern Patterns;

        public TrainStopService(IHttpService httpService, IPattern pattern)
        {
            HttpService = httpService;
            Patterns = pattern;
        }

        public async Task<IEnumerable<TrainStop>> GetTrainStop(string link)
        {
            if (!NetworkInterface.GetIsNetworkAvailable()) return null;
            var uri = new Uri("http://rasp.rw.by/m/" + ResourceLoader.Instance.Resource["Language"] + "/train/" + link);
            try
            {
                var data = await HttpService.LoadResponseAsync(uri);
                var match = Parser.ParseData(data, Patterns.TrainPointPAttern);

                return link.Contains("thread")
                    ? TrainStopGrabber.GetRegionalEconomTrainStops(match)
                    : TrainStopGrabber.GetTrainStops(match);
            }
            catch
            {
                return null;
            }
        }
    }
}
