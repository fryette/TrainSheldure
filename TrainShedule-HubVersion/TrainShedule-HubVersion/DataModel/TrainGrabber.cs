using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using TrainShedule_HubVersion.Data;

namespace TrainShedule_HubVersion.DataModel
{
    class TrainGrabber
    {
        private static IEnumerable<Match> ParseTrainData(string data)
        {
            const string pattern = "(?<startTime><div class=\"list_start\">([^<]*)<\\/?)|" +
                                   "(?<endTime><div class=\"list_end\">(.+?)<\\/?)|" +
                                   "(?<city><div class=\"list_text\">(.+?)<\\/?)|" +
                                   "(?<trainDescription><span class=\"list_text_small\">(.+?)<\\/?)|" +
                                   "<div class=\"train_type\">.+?>[s]*(?<type>[^<>]+?)[s]*<\\/div>";
            Regex rgx = new Regex(pattern, RegexOptions.Singleline);
            var match = rgx.Matches(data).Cast<Match>();
            return match;
        }

        private static string GetUrl(string fromName, string toName, string date)
        {
            return "http://rasp.rw.by/m/ru/route/?from=" + fromName + "&to=" + toName + "&date=" + date;
        }
        private static string GetHtmlCode(string url)
        {
            var httpClient = new HttpClient();
            var httpResponseMessage = httpClient.GetAsync(url).Result;
            Stream res = httpResponseMessage.Content.ReadAsStreamAsync().Result;
            StreamReader reader = new StreamReader(res, Encoding.UTF8);
            string a = reader.ReadToEnd();
            return a;
        }

        private static List<Train> GetAllTrains(IEnumerable<Match> match)
        {
            String[] train = new String[6];
            int i = 1, k = 0;
            IEnumerable<Match> enumerable = match as IList<Match> ?? match.ToList();
            var typeOftrain = GetTypeOfTrain(enumerable);
            var length = typeOftrain.Count();
            List<Train> trainList = new List<Train>();
            foreach (var m in enumerable)
            {
                if (i == 5)
                {
                    train[4] = typeOftrain[k];
                    trainList.Add(new Train(train));
                    i = 1;
                    if (++k == length) break;
                }
                switch (i)
                {
                    case 1:
                        train[0] = m.Groups[i].Value;
                        train[5] = CheckTime(m.Groups[i].Value).ToString();
                        break;
                    case 3:
                        train[i - 1] = m.Groups[i].Value.Replace("&nbsp;&mdash; ", "-");
                        break;
                    case 4:
                        train[i - 1] = m.Groups[i].Value.Replace("&nbsp;", " ");
                        break;
                    default:
                        train[i - 1] = m.Groups[i].Value;
                        break;
                }
                i++;
            }
            return trainList;
        }

        public static List<Train> GetTrainSchedure(string from, string to, string date)
        {
            return GetAllTrains(ParseTrainData(GetHtmlCode(GetUrl(from, to, date))));
        }

        private static List<string> GetTypeOfTrain(IEnumerable<Match> match)
        {
            var typesText = match.Select(x => x.Groups["type"]).Where(x => x.Value != "").Select(x => x.Value.Replace("\n\t\t", "").Replace("\t\t", ""));
            var enumerable = typesText as string[] ?? typesText.ToArray();
            List<string> imageList = new List<string>(enumerable.Count());
            foreach (var type in enumerable)
            {
                if (type.Contains("Международные"))
                    imageList.Add(@"Assets/TypeOfTrain/international-railway_80x80.jpg");
                else if (type.Contains("Межрегиональные"))
                    imageList.Add(@"/Assets/TypeOfTrain/interregional-railway_80x80.jpg");
                else if (type.Contains("Региональные"))
                    imageList.Add(@"/Assets/TypeOfTrain/regional-railway_80x80.jpg");
                else if (type.Contains("Коммерческие"))
                    imageList.Add(@"/Assets/TypeOfTrain/commercial-railway_80x80.jpg");
                else if (type.Contains("Городские"))
                    imageList.Add(@"/Assets/TypeOfTrain/cityes-railway_80x80.jpg");
                else imageList.Add("");
            }
            return imageList;
        }

        private static bool CheckTime(string time)
        {
            var myDateTime = DateTime.Parse(time);
            return myDateTime.TimeOfDay > DateTime.Now.TimeOfDay;
        }
    }
}