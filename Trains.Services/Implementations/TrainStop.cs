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
    public class TrainStop : ITrainStopService
    {

        private const string Pattern = "(?<name>class=\"list_text\">([^<]*)<\\/?)|" +
                                       "(?<startTime>class=\"list_start\">(.+?)<\\/?)|" +
                                       "(?<endTime>class=\"list_end\">(.+?)<\\/?)|" +
                                       "(?<stopTime>class=\"list_stop\">(.+?)<\\/?)";

        private readonly IAppSettings _appSettings;
        public readonly IHttpService _httpService;

        public TrainStop(IHttpService httpService, IAppSettings appSettings)
        {
            _appSettings = appSettings;
            _httpService = httpService;
        }

        public async Task<IEnumerable<Model.Entities.TrainStop>> GetTrainStop(string link)
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                var data = await _httpService.LoadResponseAsync(new Uri("http://rasp.rw.by/m/ru/train/" + link));
                var match = Parser.ParseData(data, Pattern);
                return link.Contains("thread") ? TrainStopGrabber.GetRegionalEconomTrainStops(match) : TrainStopGrabber.GetTrainStops(match);
            }
            return null;
        }
    }
}
