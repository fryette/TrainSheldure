using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;

namespace TrainScheduleBelarus.Infrastructure
{
    class TrainGrabber
    {
        private static IEnumerable<Match> ParseTrainData(string data)
        {
            const string Pattern = "(?<city>&nbsp;&mdash; (.+?)</\\/?)|" +
                                   "(?<trainDescription>train_description\">(.+?)<\\/?)|"+
                                   "(?<startTime>start-time\">([^<]*)<\\/?)|" +
                                   "(?<endTime>end-time\">(.+?)<\\/?)|" +
                                   "(?<totalTime>time-total\">(.+?)<\\/?)|" +
                                   "(?<trainNote><li class=\"train_note\">(.+?)<\\/?)|" +
                                   "(?<place>car_type=.?\">([^<]*)<\\/?)|" +
                                   "(?<trainPrice>\'price\':(.+?) /?)";
            Regex rgx = new Regex(Pattern, RegexOptions.Singleline);
            var match=rgx.Matches(data).Cast<Match>();
            return match as Match[] ?? match.ToArray();
        }

        private static string GetUrl(string fromName, string toName,string date)
        {
            return "http://rasp.rw.by/ru/route/?from=" + fromName + "&to=" + toName + "&date=" + date;
        }
        private static string GetHtmlCode(string url)
        {
            var httpClient = new HttpClient();
            var httpResponseMessage = httpClient.GetAsync(url).Result;
            Stream res = httpResponseMessage.Content.ReadAsStreamAsync().Result;
            StreamReader reader = new StreamReader(res, Encoding.UTF8);
            return reader.ReadToEnd();
        }

        public static List<Train> GetTrainSchedure(string from, string to, string date)
        {
            var match = ParseTrainData(GetHtmlCode(GetUrl(from, to, date)));
            int i = 1;
            String[] train = new String[10];
            train[9] = from;
            train[8] = to;
            List<Train> trainList = new List<Train>();
            foreach (var m in match.Skip(2)){
                if (i == 9)
                {
                    trainList.Add(new Train(train));
                    i = 1;
                }
                    train[i - 1] = m.Groups[i].Value;
                    i++;
            }
            return trainList;
        }
    }
}
