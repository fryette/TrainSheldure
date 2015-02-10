using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace TrainShedule_HubVersion.DataModel
{
    class TrainGrabber
    {
        const string Pattern = "(?<startTime><div class=\"list_start\">([^<]*)<\\/?)|" +
                                   "(?<endTime><div class=\"list_end\">(.+?)<\\/?)|" +
                                   "(?<city><div class=\"list_text\">(.+?)<\\/?)|" +
                                   "(?<trainDescription><span class=\"list_text_small\">(.+?)<\\/?)|" +
                                   "<div class=\"train_type\">.+?>[s]*(?<type>[^<>]+?)[s]*<\\/div>";

        public static IEnumerable<Train> GetTrainSchedure(string from, string to, string date, string searchParameter, bool isEconom = false)
        {
            return GetAllTrains(Parser.GetData(GetUrl(from, to, date), Pattern), searchParameter, isEconom, date);
        }
        private static string GetUrl(string fromName, string toName, string date)
        {
            return "http://rasp.rw.by/m/ru/route/?from=" + CorrectCityForRequest(fromName) + "&to=" + CorrectCityForRequest(toName) + "&date=" + date;
        }
        private static IEnumerable<Train> GetAllTrains(IEnumerable<Match> match, string searchParameter, bool isEconom, string date)
        {
            var dateOfDeparture = ParsTime(date);
            var train = new String[5];
            int i = 1, k = 0;
            var parameters = match as IList<Match> ?? match.ToList();
            var typeOftrain = GetTypeOfTrain(parameters);
            var imagePath = GetImagePath(typeOftrain);
            var length = parameters.Count() / 5;//5 parameter for search
            var trainList = new List<Train>();
            foreach (var parameter in parameters)
            {
                if (i == 5)
                {
                    trainList.Add(new Train(train) { Type = typeOftrain[k], ImagePath = imagePath[k] });
                    if (++k == length) break;
                    i = 1;
                }
                switch (i)
                {
                    case 1:
                        train[0] = parameter.Groups[i].Value;
                        var time = ParsTime(parameter.Groups[i].Value);
                        train[4] = GetBeforeDepartureTime(time, dateOfDeparture);
                        break;
                    case 3:
                        train[i - 1] = ReduceTrainName(parameter.Groups[i].Value);
                        break;
                    case 4:
                        train[i - 1] = parameter.Groups[i].Value.Replace("&nbsp;", " ");
                        break;
                    default:
                        train[i - 1] = parameter.Groups[i].Value;
                        break;
                }
                i++;
            }
            var schedule = isEconom ? trainList.Where(x => x.Type.Contains("эконом")) : trainList;
            //TODO return maybe null
            return searchParameter == "Национальный аэропорт «Минск»" || searchParameter == "Ближайшие"
                ? schedule :
                schedule.Where(x => x.Type.Contains(searchParameter));
        }

        private static List<string> GetImagePath(List<string> typeOftrain)
        {
            var imagesPath = new List<string>(typeOftrain.Count());
            foreach (var type in typeOftrain)
            {
                if (type.Contains("Международ"))
                    imagesPath.Add("Assets/Inteneshnl.png");
                else if (type.Contains("Регион"))
                    imagesPath.Add(type.Contains("бизнес") ? "Assets/Regional_biznes.png" : "Assets/Regional_econom.png");
                else if (type.Contains("Межрегион"))
                    imagesPath.Add(type.Contains("бизнес") ? "Assets/Interregional_biznes.png" : "Assets/Interregional_econom.png");
                else imagesPath.Add("Assets/Cityes.png");
            }
            return imagesPath;
        }

        private static List<string> GetTypeOfTrain(IEnumerable<Match> match)
        {
            return new List<string>(match.Select(x => x.Groups["type"]).Where(x => x.Value != "").Select(x => x.Value.Replace("\n", "").Replace("\t\t", "")));
        }

        private static bool CheckTime(DateTime time, DateTime dateTime)
        {
            return time >= DateTime.Now || time.TimeOfDay > DateTime.Now.TimeOfDay;
        }

        private static DateTime ParsTime(string time)
        {
            return DateTime.Parse(time);
        }
        private static string GetBeforeDepartureTime(DateTime time, DateTime dateToDeparture)
        {
            if (dateToDeparture >= DateTime.Now) return dateToDeparture.ToString("D", new CultureInfo("ru-ru"));
            var timeSpan = (time.TimeOfDay - DateTime.Now.TimeOfDay);
            return "через " + timeSpan.Hours + "ч. " + timeSpan.Minutes + "мин.";
        }
        private static string ReduceTrainName(string trainName)
        {
            var shortTrainName = trainName.Remove(0, trainName.IndexOf(' ')).
                Split(new[] { "&nbsp;&mdash;" }, StringSplitOptions.None).
                Aggregate("", (current, cityPoint) => current + (cityPoint.Length <= 9 ? cityPoint + "-" : cityPoint.Remove(9) + ".-"));
            return shortTrainName.Remove(shortTrainName.Length - 1);
        }
        private static string CorrectCityForRequest(string city)
        {
            if (city.Contains("Картузская")) return "Берёза-Картузская";
            if (!city.Contains("(")) return city;
            return city.Contains("Институт Культуры") ? "Институт Культуры" : city.Remove(city.IndexOf("(", StringComparison.Ordinal));
        }
    }
}