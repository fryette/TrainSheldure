using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TrainShedule_HubVersion.DataModel
{
    class Parser
    {
        private static string GetHtmlCode(string url)
        {
            var httpClient = new HttpClient();
            var httpResponseMessage = httpClient.GetAsync(url).Result;
            var res = httpResponseMessage.Content.ReadAsStreamAsync().Result;
            var reader = new StreamReader(res, Encoding.UTF8);
            return reader.ReadToEnd();
        }

        public static IEnumerable<Match> GetData(string url, string pattern)
        {
            return ParseTrainData(GetHtmlCode(url), pattern);
        }

        private static IEnumerable<Match> ParseTrainData(string data, string pattern)
        {
            var rgx = new Regex(pattern, RegexOptions.Singleline);
            return rgx.Matches(data).Cast<Match>().Where(x => !string.IsNullOrEmpty(x.Value));
        }
    }
}