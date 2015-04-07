using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Trains.Services.Infrastructure;
using Trains.Model.Entities;
using Trains.Services.Interfaces;
using Trains.Services.Tools;

namespace Trains.Services.Implementations
{
    public class TrainStopService : ITrainStopService
    {
        private const string Pattern = "(?<name>class=\"list_text\">([^<]*)<\\/?)|" +
                                       "(?<startTime>class=\"list_start\">(.+?)<\\/?)|" +
                                       "(?<endTime>class=\"list_end\">(.+?)<\\/?)|" +
                                       "(?<stopTime>class=\"list_stop\">(.+?)<\\/?)";

        public readonly IHttpService _httpService;

        public TrainStopService(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<IEnumerable<TrainStop>> GetTrainStop(string link)
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
				var uri = new Uri("http://rasp.rw.by/m/ru/train/" + link);
                var data = await _httpService.LoadResponseAsync(uri);
                var match = Parser.ParseData(data, Pattern);

                return link.Contains("thread")
						? TrainStopGrabber.GetRegionalEconomTrainStops(match)
						: TrainStopGrabber.GetTrainStops(match);
            }
            return null;
        }
    }
}
