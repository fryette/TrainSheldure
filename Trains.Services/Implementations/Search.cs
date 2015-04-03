using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trains.Infrastructure.Infrastructure;
using Trains.Model.Entities;
using Trains.Services.Interfaces;
using Trains.Entities;
using Trains.Services.Tools;
using Trains.Infrastructure.Interfaces;
using System;

namespace Trains.Services.Implementations
{
    public class Search : ISearchService
    {
        #region constant
        
        private const string Pattern = "(?<startTime><div class=\"list_start\">([^<]*)<\\/?)|" +
                                       "(?<endTime><div class=\"list_end\">(.+?)</div>)|" +
                                       "(?<city><div class=\"list_text\">(.+?)<\\/?)|" +
                                       "(?<trainDescription><span class=\"list_text_small\">(.+?)<\\/?)|" +
                                       "<div class=\"train_type\">.+?>(?<type>[^<>]+)<\\/div>";

        private const string AdditionParameterPattern = "div class=\"b-places\">(.*?)</div>";

        private const string PlacesAndPricesPattern =
            "(?<Name><td class=\"places_name\">([^<]+)</td>)|" +
            "(?<quantity><td class=\"places_qty\">([^<]*)<)|" +
            "(?<Price><td class=\"places_price\">([^<]*))";

        #endregion

        public IHttpService _httpService { get; set; }
        private readonly IAppSettings _appSettings;

        public Search(IHttpService httpService,IAppSettings appSettings)
        {
            _appSettings = appSettings;
            _httpService = httpService;
        }

        public async Task<List<Train>> GetTrainSchedule(string from, string to, string date)
        {
            var fromItem = _appSettings.AutoCompletion.First(x => x.UniqueId == from);
            var toItem = _appSettings.AutoCompletion.First(x => x.UniqueId == to);

            var data = await _httpService.LoadResponseAsync(GetUrl(fromItem, toItem, date));
            var additionalInformation = TrainGrabber.GetPlaces(data);
            var links = TrainGrabber.GetLink(data);

            IEnumerable<Train> trains;
            if (fromItem.Country != "(Беларусь)" && toItem.Country != "(Беларусь)")
                trains = TrainGrabber.GetTrainsInformationOnForeignStantion(Parser.ParseTrainData(data, Pattern).ToList(), date);
            else
                trains = date == "everyday" ? TrainGrabber.GetTrainsInformationOnAllDays(Parser.ParseTrainData(data, Pattern).ToList())
                    : TrainGrabber.GetTrainsInformation(Parser.ParseTrainData(data, Pattern).ToList(), date);

            return TrainGrabber.GetFinallyResult(additionalInformation, links, trains).ToList();
        }

        private Uri GetUrl(CountryStopPointItem fromItem, CountryStopPointItem toItem, string date)
        {
            return new Uri("http://rasp.rw.by/m/" + "ru" + "/route/?from=" +
                   fromItem.UniqueId.Split('(')[0] + "&from_exp=" + fromItem.Exp + "&to=" + toItem.UniqueId.Split('(')[0] + "&to_exp=" + toItem.Exp + "&date=" + date);
        }

        public async Task<List<Train>> UpdateTrainSchedule()
        {
            //if (_appSettings.UpdatedLastRequest == null) return null;
            //return await TrainGrabber.GetTrainSchedule(_appSettings.UpdatedLastRequest.From,
            //                    _appSettings.UpdatedLastRequest.To, ToolHelper.GetDate(_appSettings.UpdatedLastRequest.Date, _appSettings.UpdatedLastRequest.SelectionMode));
            return null;
        }

    }
}
