using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TrainShedule_HubVersion.Infrastructure
{
    internal class TrainGrabber
    {
        #region constant
        private const string Pattern = "(?<startTime><div class=\"list_start\">([^<]*)<\\/?)|" +
                                       "(?<endTime><div class=\"list_end\">(.+?)</div>)|" +
                                       "(?<city><div class=\"list_text\">(.+?)<\\/?)|" +
                                       "(?<trainDescription><span class=\"list_text_small\">(.+?)<\\/?)|" +
                                       "<div class=\"train_type\">.+?>(?<type>[^<>]+)<\\/div>";

        private const string AddiditionParameterPattern = "div class=\"b-places\">(.*?)</div>";

        private const string ParsePlacesAndPrices =
            "(?<Name><td class=\"places_name\">([^<]+)</td>)|" +
            "(?<quantity><td class=\"places_qty\">([^<]*)<)|" +
            "(?<Price><td class=\"places_price\">([^<]*))";

        private const string UnknownStr = "&nbsp;";
        private const int SearchCountParameter = 5;

        #endregion

        public static async Task<IEnumerable<Train>> GetTrainSchedule(string from, string to, string date, string searchParameter, bool isEconom, bool specialSearch)
        {
            var data = await Task.Run(() => Parser.GetHtmlCode(GetUrl(from, to, date)));
            var addiditionalInformation = GetPlaces(data, searchParameter).ToList();
            var links = GetLink(data);
            IEnumerable<Train> trains;
            var fromItem = await CountryStopPointData.GetItemByIdAsync(from);
            var toItem = await CountryStopPointData.GetItemByIdAsync(to);
            if (fromItem.Country != "(Беларусь)" && toItem.Country != "(Беларусь)")
                trains = await Task.Run(() => GetTrainsInformationOnForeignStantion(Parser.ParseTrainData(data, Pattern)));
            else
                trains = await Task.Run(() => date == "everyday" ? GetTrainsInformationOnAllDays(Parser.ParseTrainData(data, Pattern))
                    : GetTrainsInformation(Parser.ParseTrainData(data, Pattern), date));
            trains = GetFinallyResult(addiditionalInformation, links, trains);
            var schedule = specialSearch ? SearchBusinessOrEconomTrains(trains, isEconom) : trains;
            return (searchParameter == "Аэропорт" || searchParameter == "Ближайший"
                ? schedule
                : schedule.Where(x => x.Type.Contains(searchParameter))).Where(x => x.BeforeDepartureTime == null || !x.BeforeDepartureTime.Contains('-')).ToList();
        }

        private static string GetUrl(string fromName, string toName, string date)
        {
            return "http://rasp.rw.by/m/ru/route/?from=" +
                   fromName + "&to=" + toName + "&date=" + date;
        }

        private static IEnumerable<Train> GetTrainsInformation(IEnumerable<Match> match, string date)
        {
            var dateOfDeparture = DateTime.Parse(date);
            var parameters = match as IList<Match> ?? match.ToList();
            var imagePath = new List<string>(GetImagePath(parameters));
            var trainList = new List<Train>(parameters.Count / SearchCountParameter);
            var step = parameters.Count - parameters.Count / SearchCountParameter;

            for (var i = 0; i < step; i += 4)
            {
                var starTime = DateTime.Parse(parameters[i].Groups[1].Value);
                trainList.Add(CreateTrain(parameters[i].Groups[1].Value, parameters[i + 1].Groups[2].Value, parameters[i + 2].Groups[3].Value,
                    parameters[i + 3].Groups[4].Value.Replace(UnknownStr, " "), parameters[i / 4 + step].Value,
                    imagePath[i / 4], GetBeforeDepartureTime(starTime, dateOfDeparture), date));
            }
            return trainList;
        }

        private static IEnumerable<Train> GetTrainsInformationOnAllDays(IEnumerable<Match> match)
        {
            var parameters = match as IList<Match> ?? match.ToList();
            var imagePath = new List<string>(GetImagePath(parameters));
            var trainList = new List<Train>(parameters.Count / SearchCountParameter);
            var step = parameters.Count - parameters.Count / SearchCountParameter;

            for (var i = 0; i < step; i += 4)
            {
                trainList.Add(CreateTrain(parameters[i].Groups[1].Value, parameters[i + 1].Groups[2].Value,
                    parameters[i + 2].Groups[3].Value, parameters[i + 3].Groups[4].Value,
                    parameters[i / 4 + step].Value, imagePath[i / 4]));
            }
            return trainList;
        }

        private static IEnumerable<Train> GetTrainsInformationOnForeignStantion(IEnumerable<Match> match)
        {
            var parameters = match as IList<Match> ?? match.ToList();
            var trainList = new List<Train>(parameters.Count / SearchCountParameter);

            for (var i = 0; i < parameters.Count; i += 4)
            {
                trainList.Add(CreateTrain(parameters[i].Groups[1].Value, parameters[i + 1].Groups[2].Value,
                    parameters[i + 2].Groups[3].Value, parameters[i + 3].Groups[4].Value.Replace(UnknownStr, " ")));
            }
            return trainList;
        }

        private static Train CreateTrain(string time1, string time2, string city, string description = null, string type = null,
            string imagePath = null, string beforeDepartureTime = null, string departureDate = null)
        {
            var startTime = DateTime.Parse(time1);
            var endTime = DateTime.Parse(time2.Replace("<br />", " "));

            return new Train
            {
                StartTime = time1,
                EndTime = time2.Split('<')[0],
                City = city.Replace("&nbsp;&mdash;", "-"),
                Description = description,
                BeforeDepartureTime = beforeDepartureTime ?? description,
                Type = type,
                ImagePath = imagePath,
                OnTheWay = OnTheWay(startTime, endTime),
                DepartureDate = departureDate
            };
        }

        private static IEnumerable<string> GetImagePath(IEnumerable<Match> match)
        {
            return match.Select(x => x.Groups["type"].Value)
                .Where(x => !string.IsNullOrEmpty(x)).Select(type =>
                {
                    if (type.Contains("Международ"))
                        return "/Assets/Inteneshnl.png";
                    if (type.Contains("Межрегион"))
                        return type.Contains("бизнес")
                            ? "/Assets/Interregional_biznes.png"
                            : "/Assets/Interregional_econom.png";
                    if (type.Contains("Регион"))
                        return type.Contains("бизнес") ? "/Assets/Regional_biznes.png" : "/Assets/Regional_econom.png";
                    return "/Assets/Cityes.png";
                });
        }
        private static string GetBeforeDepartureTime(DateTime time, DateTime dateToDeparture)
        {
            if (dateToDeparture >= DateTime.Now) return dateToDeparture.ToString("D", new CultureInfo("ru-ru"));
            var timeSpan = (time.TimeOfDay - DateTime.Now.TimeOfDay);
            return "через " + timeSpan.Hours + "ч. " + timeSpan.Minutes + "мин.";
        }

        private static string OnTheWay(DateTime startTime, DateTime endTime)
        {
            //only on bellarusian railways
            if (endTime < startTime) endTime = endTime.AddDays(1);
            var time = endTime - startTime;
            if (time.Days == 0)
                return "В пути: " + time.Hours + "ч. " + time.Minutes + "мин.";
            return "В пути: " + (int)time.TotalHours + "ч. " + time.Minutes + "мин.";

        }

        private static IEnumerable<AdditionalInformation[]> GetPlaces(string data, string searchParameter)
        {
            var addiditionalParameter = Parser.ParseTrainData(data, AddiditionParameterPattern).ToList();
            var addiditionInformation = new List<AdditionalInformation[]>();
            if (searchParameter == "Аэропорт")
            {
                for (var i = 0; i < addiditionalParameter.Count; i++)
                {
                    addiditionInformation.Add(new[]
                    {
                        new AdditionalInformation
                        {
                            Name = "Сидячие",
                            Place = "мест:неограничено",
                            Price = "цена:неизвестно"

                        }
                    });
                }
                return addiditionInformation;
            }
            for (var i = 0; i < addiditionalParameter.Count; i++)
            {
                if (!addiditionalParameter[i].Groups[1].Value.Contains("href")) continue;
                if (i + 1 >= addiditionalParameter.Count ||
                    addiditionalParameter[i + 1].Groups[1].Value.Contains("href"))
                    addiditionInformation.Add(new[]
                    {
                        new AdditionalInformation {Name = "Мест нет. Уточняйте в кассах"}
                    });
                else
                {
                    var temp =
                        Parser.ParseTrainData(addiditionalParameter[i + 1].Groups[1].Value, ParsePlacesAndPrices)
                            .ToList();
                    var additionalInformations = new AdditionalInformation[temp.Count / 3];
                    for (var j = 0; j < temp.Count; j += 3)
                    {
                        additionalInformations[j / 3] = new AdditionalInformation
                        {
                            Name = temp[j].Groups[1].Value.Length > 18
                                ? "Сидячие"
                                : temp[j].Groups[1].Value,
                            Place = "мест: " + (temp[j + 1].Groups[2].Value == UnknownStr
                                ? "неограничено"
                                : temp[j + 1].Groups[2].Value.Replace(UnknownStr, "")),
                            Price = "цена: " + temp[j + 2].Groups[3].Value.Replace(UnknownStr, " ")
                        };
                    }
                    addiditionInformation.Add(additionalInformations);
                    ++i;
                }
            }
            return addiditionInformation;
        }
        private static List<string> GetLink(string data)
        {
            var links = Parser.ParseTrainData(data, "<a href=\"/m/ru/train/(.+?)\"").ToList();
            return links.Select(x => x.Groups[1].Value).ToList();
        }

        private static IEnumerable<Train> GetFinallyResult(IEnumerable<AdditionalInformation[]> addInformations, List<string> linksList, IEnumerable<Train> trains)
        {
            var addInf = addInformations.ToList();
            var trainsList = trains.ToList();
            for (var i = 0; i < addInf.Count; i++)
            {
                trainsList[i].AdditionalInformation = addInf[i];
                trainsList[i].Link = linksList[i];
            }
            return trainsList;
        }

        private static IEnumerable<Train> SearchBusinessOrEconomTrains(IEnumerable<Train> trains, bool isEconom)
        {
            return isEconom ? trains.Where(x => x.Type.Contains("эконом")) : trains.Where(x => x.Type.Contains("бизнес"));
        }
    }
}