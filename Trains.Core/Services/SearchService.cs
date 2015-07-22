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
using static Trains.Core.Resources.Defines.Common;

namespace Trains.Core.Services
{
	public class SearchService : ISearchService
	{
		private readonly IHttpService _httpService;
		private readonly IPattern _pattern;
		private readonly IAnalytics _analytics;


		public SearchService(IHttpService httpService, IPattern pattern, IAnalytics analytics)
		{
			_analytics = analytics;
			_httpService = httpService;
			_pattern = pattern;
		}

		public async Task<List<Train>> GetTrainSchedule(CountryStopPointItem from, CountryStopPointItem to, DateTimeOffset datum, string selectedVariant)
		{
			var date = GetDate(datum, selectedVariant);
			try
			{
				var data = await _httpService.LoadResponseAsync(GetUrl(from, to, date));

				var additionalInformation = TrainGrabber.GetPlaces(data);
				var links = TrainGrabber.GetLink(data);
				var parameters = Parser.ParseData(data, _pattern.TrainsPattern).ToList();
				var isInternetRegistration = TrainGrabber.GetInternetRegistrationsInformations(parameters);

				List<Train> trains;
				var country = ResourceLoader.Instance.Resource["Belarus"];
				if (from.Label.Contains(country) && to.Label.Contains(country))
					trains = TrainGrabber.GetTrainsInformationOnForeignStantion(parameters, date);
				else
					trains = date == "everyday" ? TrainGrabber.GetTrainsInformationOnAllDays(Parser.ParseData(data, _pattern.TrainsPattern).ToList())
						: TrainGrabber.GetTrainsInformation(parameters, date, isInternetRegistration);
				trains = TrainGrabber.GetFinallyResult(additionalInformation, links, trains).ToList();
				if (!trains.Any()) throw new ArgumentException("Bad request");
				return trains;
			}
			catch (Exception e)
			{
				_analytics.SentException(e.Message);
				_analytics.SentEvent("exceptions", "Search", $"{e.Message}---{@from.Value}{'-'}{to.Value}{':'}{selectedVariant}");
			}
			await Mvx.Resolve<IUserInteraction>().AlertAsync(ResourceLoader.Instance.Resource["TrainsNotFound"]);
			return null;
		}

		private static Uri GetUrl(CountryStopPointItem fromItem, CountryStopPointItem toItem, string date)
		{
			return new Uri(
				$"http://rasp.rw.by/m/{ResourceLoader.Instance.Resource["Language"]}/route/?from={fromItem.Value}&from_exp={fromItem.Exp}&from_esr = {fromItem.Ecp}&to={toItem.Value}&to_exp={toItem.Exp}&to_esr = {toItem.Ecp}&date={date}&{new Random().Next(0, 20)}");
		}

		private static string GetDate(DateTimeOffset datum, string selectedVariantOfSearch = null)
		{
			if (selectedVariantOfSearch == ResourceLoader.Instance.Resource["Tommorow"])
				return datum.AddDays(1).ToString(DateFormat, CultureInfo.CurrentCulture);
			if (selectedVariantOfSearch == ResourceLoader.Instance.Resource["OnAllDays"])
				return "everyday";
			if (selectedVariantOfSearch == ResourceLoader.Instance.Resource["Yesterday"])
				return datum.AddDays(-1).ToString(DateFormat, CultureInfo.CurrentCulture);
			if (selectedVariantOfSearch == ResourceLoader.Instance.Resource["OnDay"])
				return datum.ToString(DateFormat, CultureInfo.CurrentCulture);
			if (datum < DateTime.Now) datum = DateTime.Now;

			return datum.ToString(DateFormat, CultureInfo.CurrentCulture);
		}
	}
}
