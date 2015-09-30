using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Trains.Infrastructure;
using Trains.Models;

namespace Trains.Services.Services.Infrastructure
{
    public class TrainGrabber
    {
        #region constant

        private const string UnknownStr = "&nbsp;";
        private const string UnknownStr1 = "&nbsp;&mdash;";
        //images and Internet Registrations
        private const int SearchCountParameter = 2;

        #endregion

        #region action

        public static List<Train> GetTrainsInformation(List<Match> parameters, string date, List<bool> isInternetRegistration)
        {
            var dateOfDeparture = DateTime.ParseExact(date, Defines.Common.DateFormat, CultureInfo.InvariantCulture);
            var imagePath = new List<TrainClass>(GetImagePath(parameters));
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
                    isInternetRegistration[i / 4]));
            }
            return trainList;
        }

        public static List<Train> GetTrainsInformationOnAllDays(List<Match> parameters)
        {
            var imagePath = new List<TrainClass>(GetImagePath(parameters));
            var trainList = new List<Train>(parameters.Count / SearchCountParameter);
            var step = parameters.Count - imagePath.Count * 2;
            var dateNow = DateTime.Now.ToString(Defines.Common.DateFormat) + ' ';
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

        public static List<Train> GetTrainsInformationOnForeignStantion(IReadOnlyList<Match> parameters, string date)
        {
            var numberOfTrains = parameters.Count / 5;
            var trainList = new List<Train>(numberOfTrains);

            for (var i = 0; i < parameters.Count - numberOfTrains; i += 4)
            {
                trainList.Add(CreateTrain(date + ' ' + parameters[i].Groups[1].Value, parameters[i + 1].Groups[2].Value,
                    parameters[i + 2].Groups[3].Value, parameters[i + 3].Groups[4].Value.Replace(UnknownStr, string.Empty), TrainClass.Foreign, null, null, date));
            }
            return trainList;
        }

        private static Train CreateTrain(string time1, string time2, string city, string description, TrainClass image, string type = null,
             string beforeDepartureTime = null, string departureDate = null, bool internetRegistration = false)
        {
            DateTime endTime;
            DateTime startTime;
            time2 = time2.Replace("<br />", " ");
            startTime = DateTime.ParseExact(time1, Defines.Common.TimeFormat, CultureInfo.InvariantCulture);

            endTime = time2.Length > 10 ? DateTime.Parse(time2.Length == 12 ? time2 : time2.Remove(time2.Length - 1), new CultureInfo(ResourceLoader.Instance.Resource["Language"]))
                : DateTime.ParseExact(departureDate + ' ' + time2, Defines.Common.TimeFormat, CultureInfo.InvariantCulture);
            return new Train
            {
                StartTime = startTime,
                EndTime = endTime,
                City = city.Replace(UnknownStr1, " - "),
                BeforeDepartureTime = beforeDepartureTime ?? description.Replace(UnknownStr, " "),
                Type = GetType(type),
                Image = image,
                OnTheWay = departureDate == null ? null : ResourceLoader.Instance.Resource["OnWay"] + OnTheWay(startTime, endTime),
                DepartureDate = departureDate,
                InternetRegistration = internetRegistration
            };
        }

        public static List<TrainClass> GetImagePath(List<Match> match)
        {
            return match.Select(x => x.Groups["type"].Value)
                .Where(x => !string.IsNullOrEmpty(x)).Select(GetType).ToList();
        }

	    private static TrainClass GetType(string type)
	    {
			if (type.Contains(ResourceLoader.Instance.Resource["International"]))
				return TrainClass.International;
			if (type.Contains(ResourceLoader.Instance.Resource["Interregional"]))
				return type.Contains(ResourceLoader.Instance.Resource["Business"])
					? TrainClass.InterRegionalBusiness
					: TrainClass.InterRegionalEconom;
			if (type.Contains(ResourceLoader.Instance.Resource["Regional"]))
				return type.Contains(ResourceLoader.Instance.Resource["Business"])
					? TrainClass.RegionalBusiness : TrainClass.RegionalEconom;
			return TrainClass.City;
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
            var additionalParameter = Parser.ParseData(data, Defines.Common.AdditionParameterPattern).ToList();
            for (var i = 0; i < additionalParameter.Count; i++)
            {
                if (i + 1 == additionalParameter.Count ||
                    additionalParameter[i + 1].Groups[1].Value.Contains("href"))
                    additionInformation.Add(new[]
                    {
                        new AdditionalInformation {Name = ResourceLoader.Instance.Resource["NoPlace"]}
                    });
                else
                {
                    var temp =
                        Parser.ParseData(additionalParameter[i + 1].Groups[1].Value, Defines.Common.PlacesAndPricesPattern).ToList();
                    var additionalInformations = new AdditionalInformation[temp.Count / 3];
                    for (var j = 0; j < temp.Count; j += 3)
                    {
                        additionalInformations[j / 3] = new AdditionalInformation
                        {
                            Name = temp[j].Groups[1].Value.Length > 18
                                ? ResourceLoader.Instance.Resource["Sedentary"]
                                : temp[j].Groups[1].Value,
                            Place = ResourceLoader.Instance.Resource["Place"] + (temp[j + 1].Groups[2].Value == UnknownStr
                                ? ResourceLoader.Instance.Resource["Unlimited"]
                                : temp[j + 1].Groups[2].Value.Replace(UnknownStr, string.Empty)),
                            Price = temp[j + 2].Groups[3].Value == String.Empty ? ResourceLoader.Instance.Resource["Unknown"]
                                                                                  : ResourceLoader.Instance.Resource["Price"] + temp[j + 2].Groups[3].Value.Replace(UnknownStr, " ")
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
            var hours = timeSpan.Hours == 0 ? String.Empty : (timeSpan.Hours + ResourceLoader.Instance.Resource["Hour"]);
            return ResourceLoader.Instance.Resource["Via"] + hours + timeSpan.Minutes + ResourceLoader.Instance.Resource["Min"];
        }

        private static string OnTheWay(DateTime startTime, DateTime endTime)
        {
            var time = endTime - startTime;
            if (time.Days == 0)
                return time.Hours + ResourceLoader.Instance.Resource["Hour"] + time.Minutes + ResourceLoader.Instance.Resource["Min"];
            return (int)time.TotalHours + ResourceLoader.Instance.Resource["Hour"] + time.Minutes + ResourceLoader.Instance.Resource["Min"];
        }

        public static List<string> GetLink(string data)
        {
            var links = Parser.ParseData(data, "<a href=\"/m/" + ResourceLoader.Instance.Resource["Language"] + "/train/(.+?)\"");
            return links.Select(x => x.Groups[1].Value).ToList();
        }

        public static IEnumerable<Train> GetFinallyResult(IReadOnlyList<AdditionalInformation[]> additionalInformation, IReadOnlyList<string> linksList, IEnumerable<Train> trains)
        {
            var trainsList = trains.ToList();
            for (var i = 0; i < additionalInformation.Count; i++)
            {
                trainsList[i].AdditionalInformation = additionalInformation[i];
                trainsList[i].Link = linksList[i];

                if (additionalInformation[i].Any())
                    if (trainsList[i].DepartureDate != null)
                        trainsList[i].IsPlace = additionalInformation[i].First().Name.Contains(ResourceLoader.Instance.Resource["No"]) ?
                            ResourceLoader.Instance.Resource["PlaceNo"] : ResourceLoader.Instance.Resource["PlaceYes"];
                    else
                        trainsList[i].AdditionalInformation.First().Name = ResourceLoader.Instance.Resource["SpecifyDate"];

                var placeClasses = new PlaceClasses();
                foreach (var name in additionalInformation[i].Select(x => x.Name))
                {
                    if (ResourceLoader.Instance.Resource["Sedentary"] == name)
                        placeClasses.Sedentary = true;
                    else if (ResourceLoader.Instance.Resource["SecondClass"] == name)
                        placeClasses.SecondClass = true;
                    else if (ResourceLoader.Instance.Resource["Coupe"] == name)
                        placeClasses.Coupe = true;
                    else if (ResourceLoader.Instance.Resource["Luxury"] == name)
                        placeClasses.Luxury = true;
                }
                trainsList[i].PlaceClasses = placeClasses;
            }
            return trainsList.Where(x => !x.BeforeDepartureTime.Contains("-"));
        }

        #endregion
    }
}