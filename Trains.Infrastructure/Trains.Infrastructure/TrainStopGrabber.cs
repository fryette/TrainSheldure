using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Trains.Infrastructure.Interfaces;
using Trains.Model.Entities;

namespace Trains.Infrastructure
{
	public class TrainStopGrabber
	{
		private static ILocalizationService _localizationService;

		public TrainStopGrabber()
		{
			_localizationService = Dependencies.LocalizationService;
		}

		private const string Tag = "</b>";

		public static IEnumerable<TrainStop> GetTrainStops(IEnumerable<Match> match)
		{
			var parameters = match as IList<Match> ?? match.ToList();
			var trainStop = new List<TrainStop>(parameters.Count / 4);
			for (var i = 0; i < parameters.Count; i += 4)
			{
				var arrivals = parameters[i + 1].Groups[2].Value.Trim();
				var departure = parameters[i + 2].Groups[3].Value.Trim();
				var stay = parameters[i + 3].Groups[4].Value.Trim();

				trainStop.Add(new TrainStop
				{
					Name = parameters[i].Groups[1].Value,
					Arrivals = (string.IsNullOrEmpty(arrivals) || arrivals.Contains(Tag) ? null : _localizationService.GetString("Departure") + arrivals.Substring(0, 5)),
					Departures = (string.IsNullOrEmpty(departure) || departure == Tag ? null : _localizationService.GetString("Arrival") + departure),
					Stay = string.IsNullOrEmpty(stay) || stay == Tag ? null : _localizationService.GetString("Stay") + stay
				});
			}

			return trainStop;
		}
	}
}