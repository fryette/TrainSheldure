using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
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
        const string Pattern = "(?<startTime><div class=\"list_start\">([^<]*)<\\/?)|" +
                                   "(?<endTime><div class=\"list_end\">(.+?)<\\/?)|" +
                                   "(?<city><div class=\"list_text\">(.+?)<\\/?)|" +
                                   "(?<trainDescription><span class=\"list_text_small\">(.+?)<\\/?)|" +
                                   "<div class=\"train_type\">.+?>[s]*(?<type>[^<>]+?)[s]*<\\/div>";

        private static string GetUrl(string fromName, string toName, string date)
        {
            return "http://rasp.rw.by/m/ru/route/?from=" + CorrectCityForRequest(fromName) + "&to=" + CorrectCityForRequest(toName) + "&date=" + date;
        }
        private static IEnumerable<Train> GetAllTrains(IEnumerable<Match> match, string searchParameter, bool isEconom, string date)
        {
            var train = new String[7];
            var i = 1;
            var k = 0;
            var enumerable = match as IList<Match> ?? match.ToList();
            var typeOftrain = GetTypeOfTrain(enumerable);
            var length = enumerable.Count / 5;//5 parameter for search
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
                        train[5] = CheckTime(m.Groups[i].Value, DateTime.Parse(date)).ToString();
                        train[6] = GetBeforeDepartureTime(m.Groups[i].Value, DateTime.Parse(date));
                        break;
                    case 3:
                        train[i - 1] = ReduceTrainName(m.Groups[i].Value);
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
            var schedule = isEconom ? trainList.Where(x => x.Status == "True" && x.Type.Contains("эконом")) : trainList.Where(x => x.Status == "True");
            return searchParameter == "Национальный аэропорт «Минск»" || searchParameter == "Ближайшие"
                ? schedule :
                schedule.Where(x => x.Type.Contains(searchParameter));
        }

        public static IEnumerable<Train> GetTrainSchedure(string from, string to, string date, string searchParameter, bool isEconom = false)
        {
            return GetAllTrains(Parser.GetData(GetUrl(from, to, date), Pattern), searchParameter, isEconom, date);
        }

        private static List<string> GetTypeOfTrain(IEnumerable<Match> match)
        {
            return new List<string>(match.Select(x => x.Groups["type"]).Where(x => x.Value != "").Select(x => x.Value.Replace("\n\t\t", "").Replace("\t\t", "")));
        }

        private static bool CheckTime(string time, DateTime dateTime)
        {
            var myDateTime = DateTime.Parse(time);
            if (dateTime >= DateTime.Now) return true;
            return myDateTime.TimeOfDay > DateTime.Now.TimeOfDay;
        }

        private static string GetBeforeDepartureTime(string time, DateTime dateToDeparture)
        {
            var myDateTime = DateTime.Parse(time);
            if (dateToDeparture >= DateTime.Now) return "Отправление: " + dateToDeparture.ToString("D", new CultureInfo("ru-ru"));
            var timeSpan = (myDateTime.TimeOfDay - DateTime.Now.TimeOfDay);
            return "через " + timeSpan.Hours + "ч. " + timeSpan.Minutes + "мин.";
        }

        private static string ReduceTrainName(string trainName)
        {
            return trainName.Remove(0, trainName.IndexOf(' ')).
                Split(new[] { "&nbsp;&mdash;" }, StringSplitOptions.None).
                Aggregate("",(current, cityPoint) => current + (cityPoint.Length <= 10 ? cityPoint+" - " : cityPoint.Remove(10) + ". - "));
        }

        private static string CorrectCityForRequest(string city)
        {
            if (city.Contains("Картузская")) return "Берёза-Картузская";
            if (!city.Contains("(")) return city;
            return city.Contains("Институт Культуры") ? "Институт Культуры" : city.Remove(city.IndexOf("(", StringComparison.Ordinal));
        }
    }
}