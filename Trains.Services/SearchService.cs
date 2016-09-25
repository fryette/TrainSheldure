using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Chance.MvvmCross.Plugins.UserInteraction;
using Cirrious.CrossCore;
using Trains.Infrastructure;
using Trains.Infrastructure.Extensions;
using Trains.Infrastructure.Interfaces;
using Trains.Infrastructure.Interfaces.Services;
using Trains.Model;
using Trains.Model.Entities;

namespace Trains.Services
{
	public class SearchService : ISearchService
	{
		private readonly IHttpService _httpService;
		private readonly IAnalytics _analytics;
		private readonly ILocalizationService _localizationService;

		public SearchService(IHttpService httpService, IAnalytics analytics, ILocalizationService localizationService)
		{
			_analytics = analytics;
			_localizationService = localizationService;
			_httpService = httpService;
		}

		public async Task<IEnumerable<TrainModel>> GetTrainSchedule(CountryStopPointItem @from, CountryStopPointItem to, DateTimeOffset datum, string selectedVariant)
		{
			var date = GetDate(datum, selectedVariant);
			try
			{
				var data = await _httpService.LoadResponseAsync(GetUrl(from, to, date));
				if (data == null)
					return null;

				return new CustomTrainParser(_localizationService).TestMethod(data);

			}
			catch (Exception e)
			{
				_analytics.SentException(e.Message);
				_analytics.SentEvent("exceptions", "Search", $"{e.Message}---{@from.Value}{'-'}{to.Value}{':'}{selectedVariant}");
			}
			await Mvx.Resolve<IUserInteraction>().AlertAsync(_localizationService.GetString("TrainsNotFound"));
			return null;
		}

		private Uri GetUrl(CountryStopPointItem fromItem, CountryStopPointItem toItem, string date)
		{
			return new Uri(
				$"http://rasp.rw.by/m/{_localizationService.GetString("Language")}/route/?from={fromItem.Value}&from_exp={fromItem.Exp}&from_esr = {fromItem.Ecp}&to={toItem.Value}&to_exp={toItem.Exp}&to_esr = {toItem.Ecp}&date={date}&{new Random().Next(0, 20)}");
		}

		private string GetDate(DateTimeOffset datum, string selectedVariantOfSearch = null)
		{
			if (selectedVariantOfSearch == _localizationService.GetString("Tommorow"))
				return datum.AddDays(1).ToString(Defines.Common.DateFormat, CultureInfo.CurrentCulture);
			if (selectedVariantOfSearch == _localizationService.GetString("OnAllDays"))
				return "everyday";
			if (selectedVariantOfSearch == _localizationService.GetString("Yesterday"))
				return datum.AddDays(-1).ToString(Defines.Common.DateFormat, CultureInfo.CurrentCulture);
			if (selectedVariantOfSearch == _localizationService.GetString("OnDay"))
				return datum.ToString(Defines.Common.DateFormat, CultureInfo.CurrentCulture);
			if (datum < DateTime.Now) datum = DateTime.Now;

			return datum.ToString(Defines.Common.DateFormat, CultureInfo.CurrentCulture);
		}
	}
}