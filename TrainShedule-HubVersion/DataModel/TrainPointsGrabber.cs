using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;

namespace TrainShedule_HubVersion.DataModel
{
    class TrainPointsGrabber
    {
        private static IEnumerable<Match> ParseTrainData(string data)
        {
            const string pattern = @"arrStations.push\(\""(.+?)""\)";
            var rgx = new Regex(pattern,RegexOptions.Singleline);
            return rgx.Matches(data).Cast<Match>();
        }
        private static string GetHtmlCode(string url="http://www.rw.by/")
        {
            var httpClient = new HttpClient();
            var httpResponseMessage = httpClient.GetAsync(url).Result;
            var res = httpResponseMessage.Content.ReadAsStreamAsync().Result;
            var reader = new StreamReader(res, Encoding.UTF8);
            return reader.ReadToEnd();
        }
        public static IEnumerable<string> GetTrainsPoints()
        {
            return GetTrainPoints(ParseTrainData(GetHtmlCode()));
        }

        private static IEnumerable<string> GetTrainPoints(IEnumerable<Match> match)
        {
            var points = match as IList<Match> ?? match.ToList();
            return !points.Any() ? null : points.Select(point => point.Groups[1].Value).ToList();
        }
    }
}
