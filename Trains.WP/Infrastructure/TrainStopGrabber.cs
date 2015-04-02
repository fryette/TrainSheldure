using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Trains.Model.Entities;

namespace Trains.WP.Infrastructure
{
    public class TrainStopGrabber
    {
        private const string Pattern = "(?<name>class=\"list_text\">([^<]*)<\\/?)|" +
                                       "(?<startTime>class=\"list_start\">(.+?)<\\/?)|" +
                                       "(?<endTime>class=\"list_end\">(.+?)<\\/?)|" +
                                       "(?<stopTime>class=\"list_stop\">(.+?)<\\/?)";

        public static IEnumerable<TrainStop> GetTrainStop(string link)
        {
            var match = Parser.GetData("http://rasp.rw.by/m/ru/train/" + link, Pattern);
            return link.Contains("thread") ? GetRegionalEconomTrainStops(match) : GetTrainStops(match);
        }

        private static IEnumerable<TrainStop> GetTrainStops(IEnumerable<Match> match)
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
                    Arrivals = (String.IsNullOrEmpty(arrivals)? null : "Прибытие: " + arrivals.Substring(0, 5)),
                    Departures = (String.IsNullOrEmpty(departure) ? null : "Отправление: " + departure),
                    Stay = String.IsNullOrEmpty(stay) ? null : "Стоянка: " + stay
                });
            }
            return trainStop;
        }
        private static IEnumerable<TrainStop> GetRegionalEconomTrainStops(IEnumerable<Match> match)
        {
            var parameters = match as IList<Match> ?? match.ToList();
            var trainStop = new List<TrainStop>(parameters.Count / 2);
            for (var i = 0; i < parameters.Count - 2; i += 2)
            {
                trainStop.Add(new TrainStop
                {
                    Name = parameters[i + 1].Groups[1].Value,
                    Arrivals = "Отправление: " + (parameters[i].Groups[2].Value.Length > 5 ? "конечная" : parameters[i].Groups[2].Value),
                });
            }
            trainStop.Add(new TrainStop
            {
                Name = parameters[parameters.Count - 1].Groups[1].Value,
                Arrivals = "конечная",
            });
            return trainStop;
        }
    }
}
