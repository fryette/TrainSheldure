using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
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

		public async Task<List<Train>> GetTrainSchedule(CountryStopPointItem from, CountryStopPointItem to, string datum)
		{
			var date = GetDate(datum);
			try
			{
				var data = await _httpService.LoadResponseAsync(new Uri(
				$"http://rasp.rw.by/m/ru/route/?from={from.Value}&from_exp={from.Exp}&from_esr = {from.Ecp}&to={to.Value}&to_exp={to.Exp}&to_esr = {to.Ecp}&date={date}&{new Random().Next(0, 20)}"));
				if (data == null)
					return null;

				data = WebUtility.HtmlDecode(data);

				var additionalInformation = TrainGrabber.GetPlaces(data);
				var links = TrainGrabber.GetLink(data);
				var parameters = Parser.ParseData(data, Defines.Common.TrainsPattern).ToList();
				var isInternetRegistration = TrainGrabber.GetInternetRegistrationsInformations(parameters);

				List<Train> trains;
				var country = ResourceLoader.Instance.Resource["Belarus"];
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

		private static string GetDate(string date)
		{
			if (date == ResourceLoader.Instance.Resource["OnAllDays"])
				return "everyday";

			var datum = DateTimeOffset.Parse(date);

			return datum < DateTimeOffset.Now
				? DateTimeOffset.Now.ToString(Defines.Common.DateFormat) :
				datum.ToString(Defines.Common.DateFormat);
		}
	}
}
