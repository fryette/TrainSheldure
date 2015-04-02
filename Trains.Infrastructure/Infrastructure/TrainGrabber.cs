﻿using System;
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

        private const string PlacesAndPricesPattern =
            "(?<Name><td class=\"places_name\">([^<]+)</td>)|" +
            "(?<quantity><td class=\"places_qty\">([^<]*)<)|" +
            "(?<Price><td class=\"places_price\">([^<]*))";


        private const string TimeFormat = "yy-MM-dd HH:mm";

        private const string UnknownStr = "&nbsp;";
        private const int SearchCountParameter = 5;

        #endregion

        #region action

        public static IEnumerable<Train> GetTrainsInformation(IReadOnlyList<Match> parameters, string date)
        {
            var dateOfDeparture = DateTime.ParseExact(date, "yy-MM-dd", CultureInfo.InvariantCulture);
            var imagePath = new List<string>(GetImagePath(parameters));
            var trainList = new List<Train>(parameters.Count / SearchCountParameter);
            var step = parameters.Count - parameters.Count / SearchCountParameter;

            for (var i = 0; i < step; i += 4)
            {
                var starTime = DateTime.Parse(parameters[i].Groups[1].Value);
                trainList.Add(CreateTrain(date + ' ' + parameters[i].Groups[1].Value,
                    parameters[i + 1].Groups[2].Value,
                    parameters[i + 2].Groups[3].Value,
                    parameters[i + 3].Groups[4].Value.Replace(UnknownStr, " "),
                    parameters[i / 4 + step].Value,
                    imagePath[i / 4],
                    GetBeforeDepartureTime(starTime, dateOfDeparture), date));
            }
            return trainList;
        }

        public static IEnumerable<Train> GetTrainsInformationOnAllDays(IReadOnlyList<Match> parameters)
        {
            var imagePath = new List<string>(GetImagePath(parameters));
            var trainList = new List<Train>(parameters.Count / SearchCountParameter);
            var step = parameters.Count - parameters.Count / SearchCountParameter;
            var dateNow = DateTime.Now.ToString("yy-MM-dd") + ' ';
            for (var i = 0; i < step; i += 4)
            {
                trainList.Add(CreateTrain(dateNow + parameters[i].Groups[1].Value,
                    dateNow + parameters[i + 1].Groups[2].Value,
                    parameters[i + 2].Groups[3].Value,
                    parameters[i + 3].Groups[4].Value,
                    parameters[i / 4 + step].Value,
                    imagePath[i / 4]));
            }
            return trainList;
        }

        public static IEnumerable<Train> GetTrainsInformationOnForeignStantion(IReadOnlyList<Match> parameters, string date)
        {
            var trainList = new List<Train>(parameters.Count / SearchCountParameter);

            for (var i = 0; i < parameters.Count; i += 4)
            {
                trainList.Add(CreateTrain(date + ' ' + parameters[i].Groups[1].Value, parameters[i + 1].Groups[2].Value,
                    parameters[i + 2].Groups[3].Value, parameters[i + 3].Groups[4].Value.Replace(UnknownStr, string.Empty), null, "/Assets/Foreign_Trains.png"));
            }
            return trainList;
        }

        private static Train CreateTrain(string time1, string time2, string city, string description, string type = null,
            string imagePath = null, string beforeDepartureTime = null, string departureDate = null)
        {
            DateTime endTime;
            DateTime startTime;
            time2 = time2.Replace("<br />", " ");
            startTime = DateTime.ParseExact(time1, TimeFormat, CultureInfo.InvariantCulture);
            endTime = time2.Length > 10 ? DateTime.Parse(time2, CultureInfo.CurrentCulture)
                : DateTime.ParseExact(departureDate + ' ' + time2, TimeFormat, CultureInfo.InvariantCulture);
            return new Train
            {
                StartTime = startTime.ToString("t", CultureInfo.InvariantCulture),
                EndTime = endTime.ToString("t", CultureInfo.InvariantCulture),
                City = city.Replace("&nbsp;&mdash;", " - "),
                BeforeDepartureTime = beforeDepartureTime ?? description.Replace(UnknownStr, " "),
                Type = type,
                ImagePath = imagePath,
                OnTheWay = departureDate == null ? null : OnTheWay(startTime, endTime),
                DepartureDate = departureDate
            };
        }

        public static IEnumerable<string> GetImagePath(IEnumerable<Match> match)
        {
            return match.Select(x => x.Groups["type"].Value)
                .Where(x => !string.IsNullOrEmpty(x)).Select(type =>
                {
                    if (type.Contains("Международные"))
                        return "/Assets/Inteneshnl.png";
                    if (type.Contains("Межрегиональные"))
                        return type.Contains("бизнес")
                            ? "/Assets/Interregional_biznes.png"
                            : "/Assets/Interregional_econom.png";
                    if (type.Contains("региональные"))
                        return type.Contains("бизнес") ? "/Assets/Regional_biznes.png" : "/Assets/Regional_econom.png";
                    return "/Assets/Cityes.png";
                });
        }

        public static List<AdditionalInformation[]> GetPlaces(string data)
        {
            var additionInformation = new List<AdditionalInformation[]>();
            var additionalParameter = Parser.ParseTrainData(data, AdditionParameterPattern).ToList();
            for (var i = 0; i < additionalParameter.Count; i++)
            {
                if (!additionalParameter[i].Groups[1].Value.Contains("href")) continue;
                if (i + 1 == additionalParameter.Count ||
                    additionalParameter[i + 1].Groups[1].Value.Contains("href"))
                    additionInformation.Add(new[]
                    {
                        new AdditionalInformation {Name = "Мест нет. Уточняйте в кассах"}
                    });
                else
                {
                    var temp =
                        Parser.ParseTrainData(additionalParameter[i + 1].Groups[1].Value, PlacesAndPricesPattern).ToList();
                    var additionalInformations = new AdditionalInformation[temp.Count / 3];

                    for (var j = 0; j < temp.Count; j += 3)
                    {
                        additionalInformations[j / 3] = new AdditionalInformation
                        {
                            Name = temp[j].Groups[1].Value.Length > 18
                                ? "Сидячие"
                                : temp[j].Groups[1].Value,
                            Place = "Мест: " + (temp[j + 1].Groups[2].Value == UnknownStr
                                ? "неограничено"
                                : temp[j + 1].Groups[2].Value.Replace(UnknownStr, string.Empty)),
                            Price = "цена" + temp[j + 2].Groups[3].Value.Replace(UnknownStr, " ")
                        };
                    }
                    additionInformation.Add(additionalInformations);
                    ++i;
                }
            }
            return additionInformation;
        }

        private static string GetBeforeDepartureTime(DateTime time, DateTime dateToDeparture)
        {
            if (dateToDeparture >= DateTime.Now) return dateToDeparture.ToString("D");
            var timeSpan = (time.TimeOfDay - DateTime.Now.TimeOfDay);
            var hours = timeSpan.Hours == 0 ? String.Empty : (timeSpan.Hours + " ч. ");
            return "через" + hours + timeSpan.Minutes + " мин.";
        }

        private static string OnTheWay(DateTime startTime, DateTime endTime)
        {
            var time = endTime - startTime;
            if (time.Days == 0)
                return time.Hours + " ч. " + time.Minutes + " мин.";
            return (int)time.TotalHours + " ч. " + time.Minutes + " мин.";
        }

        public static List<string> GetLink(string data)
        {
            var links = Parser.ParseTrainData(data, "<a href=\"/m/" + "ru" + "/train/(.+?)\"");
            return links.Select(x => x.Groups[1].Value).ToList();
        }

        public static IEnumerable<Train> GetFinallyResult(IReadOnlyList<AdditionalInformation[]> additionalInformation, IReadOnlyList<string> linksList, IEnumerable<Train> trains)
        {
            var trainsList = trains.ToList();
            for (var i = 0; i < additionalInformation.Count; i++)
            {
                trainsList[i].AdditionalInformation = additionalInformation[i];
                trainsList[i].Link = linksList[i];
                if (trainsList[i].DepartureDate != null)
                    trainsList[i].IsPlace = additionalInformation[i].First().Name.Contains("нет") ?
                        "Мест нет" : "Места есть";
                else
                    trainsList[i].AdditionalInformation.First().Name = "Уточните дату для отображения информации о местах";
            }
            return trainsList.Where(x => !x.BeforeDepartureTime.Contains("-"));
        }

        #endregion
    }
}