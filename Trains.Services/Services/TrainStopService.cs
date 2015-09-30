using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Trains.Infrastructure;
using Trains.Infrastructure.Interfaces;
using Trains.Models;
using Trains.Services.Services.Infrastructure;

namespace Trains.Services.Services
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

				return (List<TrainStop>) TrainStopGrabber.GetTrainStops(match);
			}
			catch
			{
				return null;
			}
		}
	}
}
