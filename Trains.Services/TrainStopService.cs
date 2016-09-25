using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Trains.Infrastructure;
using Trains.Infrastructure.Extensions;
using Trains.Infrastructure.Interfaces;
using Trains.Infrastructure.Interfaces.Services;
using Trains.Model.Entities;

namespace Trains.Services
{
	public class TrainStopService : ITrainStopService
	{
		private readonly IHttpService _httpService;
		private readonly ILocalizationService _localizationService;

		public TrainStopService(IHttpService httpService, ILocalizationService localizationService)
		{
			_httpService = httpService;
			_localizationService = localizationService;
		}

		public async Task<IEnumerable<TrainStop>> GetTrainStop(string link)
		{
			if (!NetworkInterface.GetIsNetworkAvailable()) return null;
			var uri = new Uri("http://rasp.rw.by/" + link);
			try
			{
				var data = await _httpService.LoadResponseAsync(uri);
				if (data == null)
					return null;
				var match = data.ParseAsHtml(_localizationService.GetString("TrainPointPattern"));

				return TrainStopGrabber.GetTrainStops(match);
			}
			catch
			{
				return null;
			}
		}
	}
}