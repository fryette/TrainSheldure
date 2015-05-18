using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Trains.Core.Resources;
using Trains.Model.Entities;

namespace Trains.Core.Services.Infrastructure
{
    public class TrainStopGrabber
    {
        public static IEnumerable<TrainStop> GetTrainStops(IEnumerable<Match> match)
        {
            var parameters = match as IList<Match> ?? match.ToList();
            var trainStop = new List<TrainStop>(parameters.Count / 4);
            for (var i = 0; i < parameters.Count; i += 4)
            {
                var arrivals = parameters[i + 1].Groups[2].Value.Replace("\n", "").Replace("\t", "");
                var departure = parameters[i + 2].Groups[3].Value.Replace("</div>\n\t\t\t\t", "");
                var stay = parameters[i + 3].Groups[4].Value.Replace("</div>\n\t\t\t", "");

                trainStop.Add(new TrainStop
                {
                    Name = parameters[i].Groups[1].Value,
                    Arrivals = (String.IsNullOrEmpty(arrivals) ? null : ResourceLoader.Instance.Resource["Departure"] + arrivals.Substring(0, 5)),
                    Departures = (String.IsNullOrEmpty(departure) ? null : ResourceLoader.Instance.Resource["Arrival"] + departure),
                    Stay = String.IsNullOrEmpty(stay) ? null : ResourceLoader.Instance.Resource["Stay"] + stay
                });
            }
            return trainStop;
        }
    }
}
