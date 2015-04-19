using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Trains.Entities;
using Trains.Model.Entities;
using Trains.Resources;

namespace Trains.Services.Infrastructure
{
    public class TrainGrabber
    {
        #region constant

        private const string AdditionParameterPattern = "div class=\"b-places\">(.*?)</div>";

        private const string PlacesAndPricesPattern =
            "(?<Name><td class=\"places_name\">([^<]+)</td>)|" +
            "(?<quantity><td class=\"places_qty\">([^<]*)<)|" +
            "(?<Price><td class=\"places_price\">([^<]*))";


        private const string TimeFormat = "yy-MM-dd HH:mm";

        private const string UnknownStr = "&nbsp;";

        //images and Internet Registrations
        private const int SearchCountParameter = 2;

        #endregion

        #region action

        public static IEnumerable<Train> GetTrainsInformation(IReadOnlyList<Match> parameters, string date, List<bool> isInternetRegistration)
        {
            var dateOfDeparture = DateTime.ParseExact(date, Constants.DateFormat, CultureInfo.InvariantCulture);
            var imagePath = new List<Picture>(GetImagePath(parameters));
            var trainList = new List<Train>(parameters.Count / SearchCountParameter);
            var step = parameters.Count - imagePath.Count * 2;
            
            for (var i = 0; i < step; i += 4)
            {
                var starTime = DateTime.Parse(parameters[i].Groups[1].Value);
                trainList.Add(
                    CreateTrain(
                    date + ' ' + parameters[i].Groups[1].Value,
                    parameters[i + 1].Groups[2].Value,
                    parameters[i + 2].Groups[3].Value,
                    parameters[i + 3].Groups[4].Value.Replace(UnknownStr, " "),
                    imagePath[i / 4],
                    parameters[i / 4 + step].Value,
                    GetBeforeDepartureTime(starTime, dateOfDeparture),
                    date,
                    isInternetRegistration[i/4]));
            }
            return trainList;
        }

        public static IEnumerable<Train> GetTrainsInformationOnAllDays(IReadOnlyList<Match> parameters)
        {
            var imagePath = new List<Picture>(GetImagePath(parameters));
            var trainList = new List<Train>(parameters.Count / SearchCountParameter);
            var step = parameters.Count - imagePath.Count * 2;
            var dateNow = DateTime.Now.ToString(Constants.DateFormat) + ' ';
            for (var i = 0; i < step; i += 4)
            {
                trainList.Add(CreateTrain(dateNow + parameters[i].Groups[1].Value,
                    dateNow + parameters[i + 1].Groups[2].Value,
                    parameters[i + 2].Groups[3].Value,
                    parameters[i + 3].Groups[4].Value,
                    imagePath[i / 4],
                    parameters[i / 4 + step].Value));
            }
            return trainList;
        }

        public static IEnumerable<Train> GetTrainsInformationOnForeignStantion(IReadOnlyList<Match> parameters, string date)
        {
            var trainList = new List<Train>(parameters.Count / 6);

            for (var i = 0; i < parameters.Count; i += 4)
            {
                trainList.Add(CreateTrain(date + ' ' + parameters[i].Groups[1].Value, parameters[i + 1].Groups[2].Value,
                    parameters[i + 2].Groups[3].Value, parameters[i + 3].Groups[4].Value.Replace(UnknownStr, string.Empty), Picture.Foreign));
            }
            return trainList;
        }

        private static Train CreateTrain(string time1, string time2, string city, string description, Picture image, string type = null,
             string beforeDepartureTime = null, string departureDate = null, bool internetRegistration = false)
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
                Image = image,
                OnTheWay = departureDate == null ? null : "едет " + OnTheWay(startTime, endTime),
                DepartureDate = departureDate,
                InternetRegistration = internetRegistration
            };
        }

        public static IEnumerable<Picture> GetImagePath(IEnumerable<Match> match)
        {
            return match.Select(x => x.Groups["type"].Value)
                .Where(x => !string.IsNullOrEmpty(x)).Select(type =>
                {
                    if (type.Contains(ResourceLoader.Instance.Resource.GetString("International")))
                        return Picture.International;
                    if (type.Contains(ResourceLoader.Instance.Resource.GetString("Interregional")))
                        return type.Contains(ResourceLoader.Instance.Resource.GetString("Business"))
                            ? Picture.InterRegionalBusiness
                            : Picture.InterRegionalEconom;
                    if (type.Contains(ResourceLoader.Instance.Resource.GetString("Regional")))
                        return type.Contains(ResourceLoader.Instance.Resource.GetString("Business"))
                            ? Picture.RegionalBusiness : Picture.RegionalEconom;
                    return Picture.City;
                });
        }

        public static List<bool> GetInternetRegistrationsInformations(IEnumerable<Match> match)
        {
            return match.Select(x => x.Groups["internetRegistration"].Value).Where(x => !string.IsNullOrEmpty(x)).Select(m =>
            {
                if (m.Contains("item")) return true;
                return false;
            }).ToList();
        }

        public static List<AdditionalInformation[]> GetPlaces(string data)
        {
            var additionInformation = new List<AdditionalInformation[]>();
            var additionalParameter = Parser.ParseData(data, AdditionParameterPattern).ToList();
            for (var i = 0; i < additionalParameter.Count; i++)
            {
                if (i + 1 == additionalParameter.Count ||
                    additionalParameter[i + 1].Groups[1].Value.Contains("href"))
                    additionInformation.Add(new[]
                    {
                        new AdditionalInformation {Name = ResourceLoader.Instance.Resource.GetString("NoPlace")}
                    });
                else
                {
                    var temp =
                        Parser.ParseData(additionalParameter[i + 1].Groups[1].Value, PlacesAndPricesPattern).ToList();
                    var additionalInformations = new AdditionalInformation[temp.Count / 3];
                    for (var j = 0; j < temp.Count; j += 3)
                    {
                        additionalInformations[j / 3] = new AdditionalInformation
                        {
                            Name = temp[j].Groups[1].Value.Length > 18
                                ? ResourceLoader.Instance.Resource.GetString("Sessile")
                                : temp[j].Groups[1].Value,
                            Place = ResourceLoader.Instance.Resource.GetString("Place") + (temp[j + 1].Groups[2].Value == UnknownStr
                                ? ResourceLoader.Instance.Resource.GetString("Unlimited")
                                : temp[j + 1].Groups[2].Value.Replace(UnknownStr, string.Empty)),
                            Price = ResourceLoader.Instance.Resource.GetString("Price") + temp[j + 2].Groups[3].Value.Replace(UnknownStr, " ")
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
            var hours = timeSpan.Hours == 0 ? String.Empty : (timeSpan.Hours + ResourceLoader.Instance.Resource.GetString("Hour"));
            return ResourceLoader.Instance.Resource.GetString("Via") + hours + timeSpan.Minutes + ResourceLoader.Instance.Resource.GetString("Min");
        }

        private static string OnTheWay(DateTime startTime, DateTime endTime)
        {
            var time = endTime - startTime;
            if (time.Days == 0)
                return time.Hours + ResourceLoader.Instance.Resource.GetString("Hour") + time.Minutes + ResourceLoader.Instance.Resource.GetString("Min");
            return (int)time.TotalHours + ResourceLoader.Instance.Resource.GetString("Hour") + time.Minutes + ResourceLoader.Instance.Resource.GetString("Min");
        }

        public static List<string> GetLink(string data)
        {
            var links = Parser.ParseData(data, "<a href=\"/m/" + "ru" + "/train/(.+?)\"");
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
                    trainsList[i].IsPlace = additionalInformation[i].First().Name.Contains(ResourceLoader.Instance.Resource.GetString("No")) ?
                        ResourceLoader.Instance.Resource.GetString("PlaceNo") : ResourceLoader.Instance.Resource.GetString("PlaceYes");
                else
                    trainsList[i].AdditionalInformation.First().Name = ResourceLoader.Instance.Resource.GetString("SpecifyDate");
                var placeClasses = new PlaceClasses();

                foreach (var name in additionalInformation[i].Select(x => x.Name))
                {
                    switch (name)
                    {
                        case "Сидячий":
                            {
                                placeClasses.Sedentary = true;
                                break;
                            }
                        case "Общий":
                            {
                                placeClasses.General = true;
                                break;
                            }
                        case "Плацкартный":
                            {
                                placeClasses.SecondClass = true;
                                break;
                            }
                        case "Купе":
                            {
                                placeClasses.Coupe = true;
                                break;
                            }
                        case "СВ":
                            {
                                placeClasses.Luxury = true;
                                break;
                            }
                        default: { break; }
                    }
                }
                trainsList[i].PlaceClasses = placeClasses;
            }
            return trainsList.Where(x => !x.BeforeDepartureTime.Contains("-"));
        }

        #endregion
    }
}