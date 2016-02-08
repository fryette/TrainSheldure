using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Trains.Core.Interfaces;
using Trains.Core.Resources;
using Trains.Core.Services.Infrastructure;
using Trains.Core.Services.Interfaces;
using Trains.Model.Entities;

namespace Trains.Core.Services
{
	public class TrainStopService : ITrainStopService
	{
		public readonly IHttpService HttpService;
		private readonly ILocalizationService _localizationService;

		public TrainStopService(IHttpService httpService, ILocalizationService localizationService)
		{
			HttpService = httpService;
			_localizationService = localizationService;
		}

		public async Task<IEnumerable<TrainStop>> GetTrainStop(string link)
		{
			if (!NetworkInterface.GetIsNetworkAvailable()) return null;
			var uri = new Uri("http://rasp.rw.by/" + _localizationService.GetString("Language") + "/train/" + link);
			try
			{
				var data = await HttpService.LoadResponseAsync(uri);
				if (data == null)
					return null;
				var match = Parser.ParseData(data, _localizationService.GetString("TrainPointPattern"));

				return TrainStopGrabber.GetTrainStops(match);
			}
			catch
			{
				return null;
			}
		}
	}
}
