using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using TrainShedule_HubVersion.DataModel;

namespace TrainShedule_HubVersion.Infrastructure
{
    class AllTrainStop
    {
        private const string Pattern = "(?<name>class=\"list_text\">([^<]*)<\\/?)|" +
                                       "(?<startTime>class=\"list_start\">(.+?)<\\/?)|" +
                                       "(?<endTime>class=\"list_end\">(.+?)<\\/?)|" +
                                       "(?<stopTime>class=\"list_stop\">(.+?)<\\/?)";

        public static IEnumerable<TrainStop> GetTrainStop(string trainNumber, string date)
        {
            return GetTrainStops(Parser.GetData(GetUrl(trainNumber, date), Pattern));
        }

        private static string GetUrl(string trainNumber, string date)
        {
            return "http://rasp.rw.by/m/ru/train/?train=" + WebUtility.HtmlEncode(trainNumber) + "&date=" + date;
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
                    Arrivals = "Отпр: " + (arrivals == "" ? "начальная" : arrivals.Substring(0, 5)),
                    Departures = "Приб: " + (departure == "" ? "конечная" : departure),
                    Stay = "Стоянка: " + (stay == "" ? "нет" : stay)
                });
            }
            return trainStop;
        }
    }
}
