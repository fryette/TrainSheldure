using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Trains.Model.Entities;
using Trains.Resources;
using Trains.Services.Infrastructure;
using Trains.Services.Interfaces;

namespace Trains.Services
{
    public class TrainStopService : ITrainStopService
    {
        private const string Pattern = "(?<name>class=\"list_text\">([^<]*)<\\/?)|" +
                                       "(?<startTime>class=\"list_start\">(.+?)<\\/?)|" +
                                       "(?<endTime>class=\"list_end\">(.+?)<\\/?)|" +
                                       "(?<stopTime>class=\"list_stop\">(.+?)<\\/?)";

        public readonly IHttpService HttpService;

        public TrainStopService(IHttpService httpService)
        {
            HttpService = httpService;
        }

        public async Task<IEnumerable<TrainStop>> GetTrainStop(string link)
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                var uri = new Uri("http://rasp.rw.by/m/" + ResourceLoader.Instance.Resource["Language"] + "/train/" + link);
                var data = await HttpService.LoadResponseAsync(uri);
                var match = Parser.ParseData(data, Pattern);

                return link.Contains("thread")
						? TrainStopGrabber.GetRegionalEconomTrainStops(match)
						: TrainStopGrabber.GetTrainStops(match);
            }
            return null;
        }
    }
}
