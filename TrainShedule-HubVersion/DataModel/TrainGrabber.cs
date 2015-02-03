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
            var rgx = new Regex(pattern, RegexOptions.Singleline);
            var match = rgx.Matches(data).Cast<Match>();
            return match;
        }

        private static string GetUrl(string fromName, string toName, string date)
        {
            return "http://rasp.rw.by/m/ru/route/?from=" + CorrectCity(fromName) + "&to=" + CorrectCity(toName) + "&date=" + date;
        }
        private static string GetHtmlCode(string url)
        {
            var httpClient = new HttpClient();
            var httpResponseMessage = httpClient.GetAsync(url).Result;
            var res = httpResponseMessage.Content.ReadAsStreamAsync().Result;
            var reader = new StreamReader(res, Encoding.UTF8);
            return reader.ReadToEnd();
        }

        private static IEnumerable<Train> GetAllTrains(IEnumerable<Match> match)
        {

            var train = new String[7];
            var i = 1;
            var k = 0;
            var enumerable = match as IList<Match> ?? match.ToList();
            var typeOftrain = GetTypeOfTrain(enumerable);
            var length = typeOftrain.Count();
            var trainList = new List<Train>();
            foreach (var m in enumerable)
            {
                if (i == 5)
                {
                    train[4] = typeOftrain[k];
                    trainList.Add(new Train(train));
                    if (++k == length) break;
                    i = 1;
                }
                switch (i)
                {
                    case 1:
                        train[0] = m.Groups[i].Value;
                        train[5] = CheckTime(m.Groups[i].Value).ToString();
                        train[6] = GetBeforeDepartureTime(m.Groups[i].Value);
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
            return trainList.Where(x => x.Status == "True");
        }

        public static IEnumerable<Train> GetTrainSchedure(string from, string to, string date)
        {
            return GetAllTrains(ParseTrainData(GetHtmlCode(GetUrl(from, to, date))));
        }

        private static List<string> GetTypeOfTrain(IEnumerable<Match> match)
        {
            var typesText = match.Select(x => x.Groups["type"]).Where(x => x.Value != "").Select(x => x.Value.Replace("\n\t\t", "").Replace("\t\t", ""));
            var enumerable = typesText as string[] ?? typesText.ToArray();
            var imageList = new List<string>(enumerable.Count());
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

        private static string GetBeforeDepartureTime(string time)
        {
            var myDateTime = DateTime.Parse(time);
            TimeSpan timeSpan = (myDateTime.TimeOfDay - DateTime.Now.TimeOfDay);
            return timeSpan.Hours + ":" + timeSpan.Minutes;
        }

        private static string CorrectCity(string city)
        {
            if (city.Contains("Картузская")) return "Берёза-Картузская";
            if (!city.Contains("(")) return city;
            if (city.Contains("Институт Культуры")) return "Институт Культуры";
            return city.Remove(city.IndexOf("("));
        }
    }
}