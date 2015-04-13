using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trains.Services.Infrastructure;
using Trains.Model.Entities;
using Trains.Services.Interfaces;
using Trains.Entities;
using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Trains.Services.Implementations
{
    public class SearchService : ISearchService
    {
        #region constant

        private const string Pattern = "(?<startTime><div class=\"list_start\">([^<]*)<\\/?)|" +
                                       "(?<endTime><div class=\"list_end\">(.+?)</div>)|" +
                                       "(?<city><div class=\"list_text\">(.+?)<\\/?)|" +
                                       "(?<trainDescription><span class=\"list_text_small\">(.+?)<\\/?)|" +
                                       "<div class=\"train_type\">.+?>(?<type>[^<>]+)<\\/div>|" +
                                       "(?<internetRegistration><div class=\"b-legend\">(.+?)</div)";
        #endregion

        public readonly IHttpService _httpService;

        public SearchService(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<List<Train>> GetTrainSchedule(CountryStopPointItem from, CountryStopPointItem to, System.DateTimeOffset Datum, string selectedVariant)
        {
            var date = GetDate(Datum, selectedVariant);
            string data;

            try
            {
                data = await _httpService.LoadResponseAsync(GetUrl(from, to, date));
                var additionalInformation = TrainGrabber.GetPlaces(data);
                var links = TrainGrabber.GetLink(data);
                var parameters = Parser.ParseData(data, Pattern).ToList();
                var isInternetRegistration = TrainGrabber.GetInternetRegistrationsInformations(parameters);

                IEnumerable<Train> trains;
                if (from.Country != "(Беларусь)" && to.Country != "(Беларусь)")
                    trains = TrainGrabber.GetTrainsInformationOnForeignStantion(parameters, date);
                else
                    trains = date == "everyday" ? TrainGrabber.GetTrainsInformationOnAllDays(Parser.ParseData(data, Pattern).ToList())
                        : TrainGrabber.GetTrainsInformation(parameters, date, isInternetRegistration);

                return TrainGrabber.GetFinallyResult(additionalInformation, links, trains).ToList();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private Uri GetUrl(CountryStopPointItem fromItem, CountryStopPointItem toItem, string date)
        {
            return new Uri("http://rasp.rw.by/m/" + "ru" + "/route/?from=" +
                   fromItem.UniqueId.Split('(')[0] + "&from_exp=" + fromItem.Exp + "&to=" + toItem.UniqueId.Split('(')[0] + "&to_exp=" + toItem.Exp + "&date=" + date + "&" + new Random().Next(0, 20));
        }

        private string GetDate(DateTimeOffset datum, string selectedVariantOfSearch = null)
        {
            if (selectedVariantOfSearch == "Завтра") return datum.AddDays(1).ToString("yy-MM-dd", CultureInfo.CurrentCulture);
            if (selectedVariantOfSearch == "Все дни") return "everyday";
            if (selectedVariantOfSearch == "Вчера")
                return datum.AddDays(-1).ToString("yy-MM-dd", CultureInfo.CurrentCulture);

            if (datum < DateTime.Now) datum = DateTime.Now;
            return datum.ToString("yy-MM-dd", CultureInfo.CurrentCulture);
        }
    }
}
