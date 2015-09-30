using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Trains.Infrastructure;
using Trains.Infrastructure.Interfaces;
using Trains.Models;
using Trains.Services.Services.Infrastructure;

namespace Trains.Services.Services
{
	public class SearchService : ISearchService
	{
		private readonly IHttpService _httpService;
		//private readonly IAnalytics _analytics;

		public SearchService(IHttpService httpService)
		{
			//_analytics = analytics;
			_httpService = httpService;
		}

		public async Task<List<Train>> GetTrainSchedule(CountryStopPointItem from, CountryStopPointItem to, DateTimeOffset datum, string selectedVariant)
		{
			var date = datum.AddDays(1).ToString(Defines.Common.DateFormat, CultureInfo.CurrentCulture);
			try
			{
				var data = await _httpService.LoadResponseAsync(GetUrl(from, to, date));
				if (data == null)
					return null;

				var additionalInformation = TrainGrabber.GetPlaces(data);
				var links = TrainGrabber.GetLink(data);
				var parameters = Parser.ParseData(data, Defines.Common.TrainsPattern).ToList();
				var isInternetRegistration = TrainGrabber.GetInternetRegistrationsInformations(parameters);

				List<Train> trains;
				var country = "Беларусь";
				if (!from.Label.Contains(country) && !to.Label.Contains(country))
					trains = TrainGrabber.GetTrainsInformationOnForeignStantion(parameters, date);
				else
					trains = date == "everyday" ? TrainGrabber.GetTrainsInformationOnAllDays(Parser.ParseData(data, Defines.Common.TrainsPattern).ToList())
						: TrainGrabber.GetTrainsInformation(parameters, date, isInternetRegistration);
				trains = TrainGrabber.GetFinallyResult(additionalInformation, links, trains).ToList();
				if (!trains.Any()) throw new ArgumentException("Bad request");
				return trains;
			}
			catch (Exception e)
			{
				//_analytics.SentException(e.Message);
				//_analytics.SentEvent("exceptions", "Search", $"{e.Message}---{@from.Value}{'-'}{to.Value}{':'}{selectedVariant}");
			}
			//await Mvx.Resolve<IUserInteraction>().AlertAsync(ResourceLoader.Instance.Resource["TrainsNotFound"]);
			return null;
		}

		private static Uri GetUrl(CountryStopPointItem fromItem, CountryStopPointItem toItem, string date)
		{
			return new Uri(
				$"http://rasp.rw.by/m/ru/route/?from={fromItem.Value}&from_exp={fromItem.Exp}&from_esr = {fromItem.Ecp}&to={toItem.Value}&to_exp={toItem.Exp}&to_esr = {toItem.Ecp}&date={date}&{new Random().Next(0, 20)}");
		}

		//private static string GetDate(DateTimeOffset datum, string selectedVariantOfSearch = null)
		//{
		//	if (selectedVariantOfSearch == ResourceLoader.Instance.Resource["Tommorow"])
		//		return datum.AddDays(1).ToString(Defines.Common.DateFormat, CultureInfo.CurrentCulture);
		//	if (selectedVariantOfSearch == ResourceLoader.Instance.Resource["OnAllDays"])
		//		return "everyday";
		//	if (selectedVariantOfSearch == ResourceLoader.Instance.Resource["Yesterday"])
		//		return datum.AddDays(-1).ToString(Defines.Common.DateFormat, CultureInfo.CurrentCulture);
		//	if (selectedVariantOfSearch == ResourceLoader.Instance.Resource["OnDay"])
		//		return datum.ToString(Defines.Common.DateFormat, CultureInfo.CurrentCulture);
		//	if (datum < DateTime.Now) datum = DateTime.Now;

		//	return datum.ToString(Defines.Common.DateFormat, CultureInfo.CurrentCulture);
		//}
	}
}
