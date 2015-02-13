using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TrainShedule_HubVersion.DataModel
{
    internal class TrainGrabber
    {
        private const string Pattern = "(?<startTime><div class=\"list_start\">([^<]*)<\\/?)|" +
                                       "(?<endTime><div class=\"list_end\">(.+?)<\\/?)|" +
                                       "(?<city><div class=\"list_text\">(.+?)<\\/?)|" +
                                       "(?<trainDescription><span class=\"list_text_small\">(.+?)<\\/?)|" +
                                       "<div class=\"train_type\">.+?>(?<type>[^<>]+)<\\/div>";

        public static Task<IEnumerable<Train>> GetTrainSchedule(string from, string to, string date,
            string searchParameter, bool isEconom = false)
        {
            return
                Task.Run(
                    () => GetAllTrains(Parser.GetData(GetUrl(from, to, date), Pattern), searchParameter, isEconom, date));
        }

        private static string GetUrl(string fromName, string toName, string date)
        {
            return "http://rasp.rw.by/m/ru/route/?from=" +
                   fromName + "&to=" + toName + "&date=" + date;
        }

        private static IEnumerable<Train> GetAllTrains(IEnumerable<Match> match, string searchParameter, bool isEconom,
            string date)
        {
            var dateOfDeparture = DateTime.Parse(date);
            var parameters = match as IList<Match> ?? match.ToList();
            var imagePath = new List<string>(GetImagePath(parameters));
            var trainList = new List<Train>(parameters.Count / 5);
            var step = parameters.Count - parameters.Count / 5;

            for (var i = 0; i < step; i += 4)
            {
                trainList.Add(new Train
                {
                    StartTime = parameters[i].Groups[1].Value,
                    EndTime = parameters[i + 1].Groups[2].Value,
                    City = ReduceTrainName(parameters[i + 2].Groups[3].Value),
                    Description = parameters[i + 3].Groups[4].Value.Replace("&nbsp;", " "),
                    BeforeDepartureTime =
                        GetBeforeDepartureTime(DateTime.Parse(parameters[i].Groups[1].Value), dateOfDeparture),
                    Type = parameters[i / 4 + step].Groups[5].Value,
                    ImagePath = imagePath[i / 4]
                });
            }

            var schedule = isEconom ? trainList.Where(x => x.Type.Contains("эконом")) : trainList;
            return searchParameter == "Национальный аэропорт «Минск»" || searchParameter == "Ближайшие"
                ? schedule
                : schedule.Where(x => x.Type.Contains(searchParameter));
        }

        private static IEnumerable<string> GetImagePath(IEnumerable<Match> match)
        {
            return match.Select(x => x.Groups["type"].Value)
                .Where(x => !string.IsNullOrEmpty(x)).Select(type =>
                {
                    if (type.Contains("Международ"))
                        return "Assets/Inteneshnl.png";
                    if (type.Contains("Регион"))
                        return type.Contains("бизнес") ? "Assets/Regional_biznes.png" : "Assets/Regional_econom.png";
                    if (type.Contains("Межрегион"))
                        return type.Contains("бизнес")
                            ? "Assets/Interregional_biznes.png"
                            : "Assets/Interregional_econom.png";
                    return "Assets/Cityes.png";
                });
        }

        private static string GetBeforeDepartureTime(DateTime time, DateTime dateToDeparture)
        {
            if (dateToDeparture >= DateTime.Now) return dateToDeparture.ToString("D", new CultureInfo("ru-ru"));
            var timeSpan = (time.TimeOfDay - DateTime.Now.TimeOfDay);
            return "через " + timeSpan.Hours + "ч. " + timeSpan.Minutes + "мин.";
        }

        private static string ReduceTrainName(string trainName)
        {
            var shortTrainName = trainName
                .Remove(0, trainName.IndexOf(' '))
                .Split(new[] { "&nbsp;&mdash;" }, StringSplitOptions.None)
                .Aggregate("",
                    (current, cityPoint) =>
                        current + (cityPoint.Length <= 9 ? cityPoint + "-" : cityPoint.Remove(9) + ".-"));
            return shortTrainName.Remove(shortTrainName.Length - 1);
        }
    }
}