using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Trains.Model.Entities;
using Trains.Entities;
namespace Trains.Infrastructure.Infrastructure
{
    public class TrainGrabber
    {
        #region constant
        private const string Pattern = "(?<startTime><div class=\"list_start\">([^<]*)<\\/?)|" +
                                       "(?<endTime><div class=\"list_end\">(.+?)</div>)|" +
                                       "(?<city><div class=\"list_text\">(.+?)<\\/?)|" +
                                       "(?<trainDescription><span class=\"list_text_small\">(.+?)<\\/?)|" +
                                       "<div class=\"train_type\">.+?>(?<type>[^<>]+)<\\/div>";

        private const string AdditionParameterPattern = "div class=\"b-places\">(.*?)</div>";

        private const string ParsePlacesAndPrices =
            "(?<Name><td class=\"places_name\">([^<]+)</td>)|" +
            "(?<quantity><td class=\"places_qty\">([^<]*)<)|" +
            "(?<Price><td class=\"places_price\">([^<]*))";

        private const string BelarusConstString = "(Беларусь)";
        private const string EveryDay = "everyday";

        private const string UnknownStr = "&nbsp;";
        private const int SearchCountParameter = 5;

        #endregion

        public static async Task<List<Train>> GetTrainSchedule(string from, string to, string date)
        {
            var fromItem = await CountryStopPointData.GetItemByIdAsync(from);
            var toItem = await CountryStopPointData.GetItemByIdAsync(to);

            var data = Parser.GetHtmlCode(GetUrl(fromItem, toItem, date));
            var additionalInformation = GetPlaces(data);
            var links = GetLink(data);

            IEnumerable<Train> trains;
            if (fromItem.Country != BelarusConstString && toItem.Country != BelarusConstString)
                trains = GetTrainsInformationOnForeignStantion(Parser.ParseTrainData(data, Pattern).ToList(), date);
            else
                trains = date == EveryDay ? GetTrainsInformationOnAllDays(Parser.ParseTrainData(data, Pattern).ToList())
                    : GetTrainsInformation(Parser.ParseTrainData(data, Pattern).ToList(), date);

            return GetFinallyResult(additionalInformation, links, trains).ToList();
        }

        private static string GetUrl(CountryStopPointDataItem fromItem, CountryStopPointDataItem toItem, string date)
        {
            return "http://rasp.rw.by/m/ru/route/?from=" +
                   fromItem.UniqueId.Split('(')[0] + "&from_exp=" + fromItem.Exp + "&to=" + toItem.UniqueId.Split('(')[0] + "&to_exp=" + toItem.Exp + "&date=" + date;
        }

        private static IEnumerable<Train> GetTrainsInformation(IReadOnlyList<Match> parameters, string date)
        {
            var dateOfDeparture = DateTime.Parse(date);
            var imagePath = new List<string>(GetImagePath(parameters));
            var trainList = new List<Train>(parameters.Count / SearchCountParameter);
            var step = parameters.Count - parameters.Count / SearchCountParameter;

            for (var i = 0; i < step; i += 4)
            {
                var starTime = DateTime.Parse(parameters[i].Groups[1].Value);
                trainList.Add(CreateTrain(date + " " + parameters[i].Groups[1].Value, parameters[i + 1].Groups[2].Value, parameters[i + 2].Groups[3].Value,
                    parameters[i + 3].Groups[4].Value.Replace(UnknownStr, " "), parameters[i / 4 + step].Value,
                    imagePath[i / 4], GetBeforeDepartureTime(starTime, dateOfDeparture), date));
            }
            return trainList;
        }

        private static IEnumerable<Train> GetTrainsInformationOnAllDays(IReadOnlyList<Match> parameters)
        {
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

        private static IEnumerable<Train> GetTrainsInformationOnForeignStantion(IReadOnlyList<Match> parameters, string date)
        {
            var trainList = new List<Train>(parameters.Count / SearchCountParameter);

            for (var i = 0; i < parameters.Count; i += 4)
            {
                trainList.Add(CreateTrain(date + " " + parameters[i].Groups[1].Value, parameters[i + 1].Groups[2].Value,
                    parameters[i + 2].Groups[3].Value, parameters[i + 3].Groups[4].Value.Replace(UnknownStr, ""), null, "/Assets/Foreign_Trains.png"));
            }
            return trainList;
        }

        private static Train CreateTrain(string time1, string time2, string city, string description, string type = null,
            string imagePath = null, string beforeDepartureTime = null, string departureDate = null)
        {
            var startTime = DateTime.Parse(time1);
            DateTime endTime;
            endTime = DateTime.Parse(time2.Replace("<br />", " ") + (time2.Length > 12 ? "" : " " + departureDate));
            return new Train
            {
                StartTime = time1.Contains(' ') ? time1.Split(' ')[1] : time1,
                EndTime = time2.Split('<')[0],
                City = city.Replace("&nbsp;&mdash;", " - "),
                BeforeDepartureTime = beforeDepartureTime ?? description.Replace(UnknownStr, " "),
                Type = type,
                ImagePath = imagePath,
                OnTheWay = departureDate == null ? "" : OnTheWay(startTime, endTime),
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

        private static List<AdditionalInformation[]> GetPlaces(string data)
        {
            var additionInformation = new List<AdditionalInformation[]>();
            var additionalParameter = Parser.ParseTrainData(data, AdditionParameterPattern).ToList();
            for (var i = 0; i < additionalParameter.Count; i++)
            {
                if (!additionalParameter[i].Groups[1].Value.Contains("href")) continue;
                if (i + 1 >= additionalParameter.Count ||
                    additionalParameter[i + 1].Groups[1].Value.Contains("href"))
                    additionInformation.Add(new[]
                    {
                        new AdditionalInformation {Name = "Мест нет. Уточняйте в кассах"}
                    });
                else
                {
                    var temp =
                        Parser.ParseTrainData(additionalParameter[i + 1].Groups[1].Value, ParsePlacesAndPrices)
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
                    additionInformation.Add(additionalInformations);
                    ++i;
                }
            }
            return additionInformation;
        }

        private static IEnumerable<AdditionalInformation[]> GetAirportPlaces(int count)
        {
            var additionInformation = new List<AdditionalInformation[]>();
            for (var i = 0; i < count; i++)
            {
                additionInformation.Add(new[]
                    {
                        new AdditionalInformation
                        {
                            Name = "Сидячие",
                            Place = "мест:неограничено",
                            Price = "цена:неизвестно"

                        }
                    });
            }
            return additionInformation;
        }

        private static string GetBeforeDepartureTime(DateTime time, DateTime dateToDeparture)
        {
            if (dateToDeparture >= DateTime.Now) return dateToDeparture.ToString("D", new CultureInfo("ru-ru"));
            var timeSpan = (time.TimeOfDay - DateTime.Now.TimeOfDay);
            return "через " + timeSpan.Hours + " ч. " + timeSpan.Minutes + " мин.";
        }

        private static string OnTheWay(DateTime startTime, DateTime endTime)
        {
            var time = endTime - startTime;
            if (time.Days == 0)
                return time.Hours + "ч. " + time.Minutes + "мин.";
            return (int)time.TotalHours + "ч. " + time.Minutes + "мин.";
        }

        private static List<string> GetLink(string data)
        {
            var links = Parser.ParseTrainData(data, "<a href=\"/m/ru/train/(.+?)\"").ToList();
            return links.Select(x => x.Groups[1].Value).ToList();
        }

        private static IEnumerable<Train> GetFinallyResult(List<AdditionalInformation[]> additionalInformation, List<string> linksList, IEnumerable<Train> trains)
        {
            var trainsList = trains.ToList();
            for (var i = 0; i < additionalInformation.Count; i++)
            {
                trainsList[i].AdditionalInformation = additionalInformation[i];
                trainsList[i].Link = linksList[i];
                if (trainsList[i].DepartureDate != null)
                    trainsList[i].IsPlace = additionalInformation[i].First().Name.Contains("нет") ? "Мест нет" : "Места есть";
                else
                {
                    trainsList[i].IsPlace = "Мест: уточните дату";
                }
            }

            return trainsList.Where(x => !x.BeforeDepartureTime.Contains('-'));
        }

    }
}