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
            string pattern = "(?<city>&nbsp;&mdash; (.+?)</\\/?)|" +
                                   "(?<trainDescription>train_description\">(.+?)<\\/?)|"+
                                   "(?<startTime>start-time\">([^<]*)<\\/?)|" +
                                   "(?<endTime>end-time\">(.+?)<\\/?)|" +
                                   "(?<totalTime>time-total\">(.+?)<\\/?)|" +
                                   "(?<trainNote><li class=\"train_note\">(.+?)<\\/?)|" +
                                   "(?<place>car_type=.?\">([^<]*)<\\/?)|" +
                                   "(?<trainPrice>\'price\':(.+?) /?)";
            Regex rgx = new Regex(pattern, RegexOptions.Singleline);
            var match=rgx.Matches(data).Cast<Match>();
            return match;
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

        private static List<Train> GetAllTrains(string from,string to,IEnumerable<Match> match)
        {
            String[] train = new String[10];
            train[9] = from;
            train[8] = to;
            int i = 1;
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

        public static List<Train> GetTrainSchedure(string from, string to, string date)
        {
            return GetAllTrains(from,to,ParseTrainData(GetHtmlCode(GetUrl(from, to, date))));
        }
    }
}
