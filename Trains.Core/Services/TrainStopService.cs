using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Trains.Core.Resources;
using Trains.Core.Services.Infrastructure;
using Trains.Core.Services.Interfaces;
using Trains.Model.Entities;

namespace Trains.Core.Services
{
	public class TrainStopService : ITrainStopService
	{
		public readonly IHttpService HttpService;

		public TrainStopService(IHttpService httpService)
		{
			HttpService = httpService;
		}

		public async Task<IEnumerable<TrainStop>> GetTrainStop(string link)
		{
			if (!NetworkInterface.GetIsNetworkAvailable()) return null;
			var uri = new Uri("http://rasp.rw.by/" + ResourceLoader.Instance.Resource["Language"] + "/train/" + link);
			try
			{
				var data = await HttpService.LoadResponseAsync(uri);
				if (data == null)
					return null;
				var match = Parser.ParseData(data, ResourceLoader.Instance.Resource["TrainPointPattern"]);

				return TrainStopGrabber.GetTrainStops(match);
			}
			catch
			{
				return null;
			}
		}
	}
}
