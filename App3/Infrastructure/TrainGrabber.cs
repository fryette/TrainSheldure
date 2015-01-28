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
            string pattern = "(?<startTime><div class=\"list_start\">([^<]*)<\\/?)|" +
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
            String[] train = new String[5];
            int i = 1,k=0;
            var length=match.Count();
            var typeOftrain = GetTypeOfTrain(match);
            List<Train> trainList = new List<Train>();
            foreach (var m in match)
            {
                if (i == 5)
                {
                    if ((k+1) == length/5) break;
                    train[4] = typeOftrain[k];
                    trainList.Add(new Train(train));
                    k++;
                    i = 1;
                }
                if (i == 3) train[i - 1] = m.Groups[i].Value.Replace("&nbsp;&mdash; ", "-");
                else if (i == 4) train[i - 1] = m.Groups[i].Value.Replace("&nbsp;", " ");
                else train[i - 1] = m.Groups[i].Value;
                i++;
            }
            return trainList;
        }

        public static List<Train> GetTrainSchedure(string from, string to, string date)
        {
            return GetAllTrains(ParseTrainData(GetHtmlCode(GetUrl(from, to, date))));
        }
        public static List<string> GetTypeOfTrain(IEnumerable<Match> match)
        {
            var typesText= match.Select(x => x.Groups["type"]).Where(x => x.Value != "").Select(x => x.Value.Replace("\n\t\t", "").Replace("\t\t", ""));
            List<string> imageList=new List<string>(typesText.Count());
            foreach (var type in typesText)
            {
                if (type.Contains("Международные"))
                    imageList.Add(@"Assets/TypeOfTrain/international-railway_80x80.jpg");
                else if (type.Contains("Межрегиональные"))
                    imageList.Add(@"/Assets/TypeOfTrain/interregional-railway_80x80.jpg");
                else if(type.Contains("Региональные"))
                    imageList.Add(@"/Assets/TypeOfTrain/regional-railway_80x80.jpg");
                else if (type.Contains("Коммерческие"))
                    imageList.Add(@"/Assets/TypeOfTrain/commercial-railway_80x80.jpg");
                else if(type.Contains("Городские"))
                    imageList.Add(@"/Assets/TypeOfTrain/cityes-railway_80x80.jpg");
                else imageList.Add("");
            }
            return imageList;
        }
    }
}
