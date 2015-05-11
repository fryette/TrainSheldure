using Chance.MvvmCross.Plugins.UserInteraction;
using Cirrious.CrossCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Trains.Core.Interfaces;
using Trains.Core.Resources;
using Trains.Core.Services.Infrastructure;
using Trains.Core.Services.Interfaces;
using Trains.Entities;
using Trains.Model.Entities;

namespace Trains.Core.Services
{
    public class SearchService : ISearchService
    {
        private readonly IHttpService HttpService;
        private readonly IPattern Pattern;
        private readonly IAnalytics Analytics;


        public SearchService(IHttpService httpService, IPattern pattern, IAnalytics analytics)
        {
            Analytics = analytics;
            HttpService = httpService;
            Pattern = pattern;
        }

        public async Task<List<Train>> GetTrainSchedule(CountryStopPointItem from, CountryStopPointItem to, DateTimeOffset datum, string selectedVariant)
        {
            var date = GetDate(datum, selectedVariant);
            try
            {
                var data = await HttpService.LoadResponseAsync(GetUrl(from, to, date));

                var additionalInformation = TrainGrabber.GetPlaces(data);
                var links = TrainGrabber.GetLink(data);
                var parameters = Parser.ParseData(data, Pattern.TrainsPattern).ToList();
                var isInternetRegistration = TrainGrabber.GetInternetRegistrationsInformations(parameters);

                List<Train> trains;
                if (from.Country != ResourceLoader.Instance.Resource["Belarus"] && to.Country != ResourceLoader.Instance.Resource["Belarus"])
                    trains = TrainGrabber.GetTrainsInformationOnForeignStantion(parameters, date);
                else
                    trains = date == "everyday" ? TrainGrabber.GetTrainsInformationOnAllDays(Parser.ParseData(data, Pattern.TrainsPattern).ToList())
                        : TrainGrabber.GetTrainsInformation(parameters, date, isInternetRegistration);
                trains = TrainGrabber.GetFinallyResult(additionalInformation, links, trains).ToList();
                if (!trains.Any()) throw new ArgumentException("Bad request");
                return trains;
            }
            catch (Exception e)
            {
                Analytics.SentException(e.Message + from.UniqueId + '-' + to.UniqueId + ':' + selectedVariant);
            }
            await Mvx.Resolve<IUserInteraction>().AlertAsync(ResourceLoader.Instance.Resource["TrainsNotFound"]);
            return null;
        }

        private Uri GetUrl(CountryStopPointItem fromItem, CountryStopPointItem toItem, string date)
        {
            return new Uri("http://rasp.rw.by/m/" + ResourceLoader.Instance.Resource["Language"] + "/route/?from=" +
                   fromItem.UniqueId.Split('(')[0] + "&from_exp=" + fromItem.Exp + "&to=" + toItem.UniqueId.Split('(')[0] + "&to_exp=" + toItem.Exp + "&date=" + date + "&" + new Random().Next(0, 20));
        }

        private string GetDate(DateTimeOffset datum, string selectedVariantOfSearch = null)
        {
            if (selectedVariantOfSearch == ResourceLoader.Instance.Resource["Tommorow"])
                return datum.AddDays(1).ToString("yy-MM-dd", CultureInfo.CurrentCulture);
            if (selectedVariantOfSearch == ResourceLoader.Instance.Resource["OnAllDays"])
                return "everyday";
            if (selectedVariantOfSearch == ResourceLoader.Instance.Resource["Yesterday"])
                return datum.AddDays(-1).ToString("yy-MM-dd", CultureInfo.CurrentCulture);
            if (selectedVariantOfSearch == ResourceLoader.Instance.Resource["OnDay"])
                return datum.ToString("yy-MM-dd", CultureInfo.CurrentCulture);

            if (datum < DateTime.Now) datum = DateTime.Now;
            return datum.ToString("yy-MM-dd", CultureInfo.CurrentCulture);
        }
    }
}
