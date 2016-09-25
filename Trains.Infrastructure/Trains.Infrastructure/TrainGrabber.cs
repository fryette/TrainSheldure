//using System;
//using System.Collections.Generic;
//using System.Globalization;
//using System.Linq;
//using System.Text.RegularExpressions;
//using Trains.Infrastructure.Extensions;
//using Trains.Infrastructure.Interfaces;
//using Trains.Model.Entities;

//namespace Trains.Infrastructure
//{
//	public class TrainGrabber
//	{
//		private static readonly ILocalizationService _localizationService;

//		static TrainGrabber()
//		{
//			_localizationService = Dependencies.LocalizationService;
//		}

//		#region constant

//		private const string UnknownStr = "&nbsp;";
//		private const string UnknownStr1 = "&nbsp;&mdash;";
//		//images and Internet Registrations
//		private const int SearchCountParameter = 2;

//		#endregion

//		#region action

//		public static List<TrainModel> GetTrainsInformation(List<Match> parameters, string date, List<bool> isInternetRegistration)
//		{
//			var dateOfDeparture = DateTime.ParseExact(date, Defines.Common.DateFormat, CultureInfo.InvariantCulture);
//			var imagePath = new List<TrainClass>(GetImagePath(parameters));
//			var trainList = new List<Train>(parameters.Count/SearchCountParameter);
//			var step = parameters.Count - imagePath.Count*2;

//			for (var i = 0; i < step; i += 4)
//			{
//				var starTime = DateTime.Parse(parameters[i].Groups[1].Value);
//				trainList.Add(
//					CreateTrain(
//						date + ' ' + parameters[i].Groups[1].Value,
//						parameters[i + 1].Groups[2].Value,
//						parameters[i + 2].Groups[3].Value,
//						parameters[i + 3].Groups[4].Value.Replace(UnknownStr, " "),
//						imagePath[i/4],
//						parameters[i/4 + step].Value,
//						GetBeforeDepartureTime(starTime, dateOfDeparture),
//						date,
//						isInternetRegistration[i/4]));
//			}
//			return trainList;
//		}

//		public static List<Train> GetTrainsInformationOnAllDays(List<Match> parameters)
//		{
//			var imagePath = new List<TrainClass>(GetImagePath(parameters));
//			var trainList = new List<Train>(parameters.Count/SearchCountParameter);
//			var step = parameters.Count - imagePath.Count*2;
//			var dateNow = DateTime.Now.ToString(Defines.Common.DateFormat) + ' ';
//			for (var i = 0; i < step; i += 4)
//			{
//				trainList.Add(
//					CreateTrain(
//						dateNow + parameters[i].Groups[1].Value,
//						dateNow + parameters[i + 1].Groups[2].Value,
//						parameters[i + 2].Groups[3].Value,
//						parameters[i + 3].Groups[4].Value,
//						imagePath[i/4],
//						parameters[i/4 + step].Value));
//			}
//			return trainList;
//		}

//		public static List<Train> GetTrainsInformationOnForeignStantion(IReadOnlyList<Match> parameters, string date)
//		{
//			var numberOfTrains = parameters.Count/5;
//			var trainList = new List<Train>(numberOfTrains);

//			for (var i = 0; i < parameters.Count - numberOfTrains; i += 4)
//			{
//				trainList.Add(
//					CreateTrain(
//						date + ' ' + parameters[i].Groups[1].Value,
//						parameters[i + 1].Groups[2].Value,
//						parameters[i + 2].Groups[3].Value,
//						parameters[i + 3].Groups[4].Value.Replace(UnknownStr, string.Empty),
//						TrainClass.FOREIGN,
//						null,
//						null,
//						date));
//			}
//			return trainList;
//		}

//		private static Train CreateTrain(
//			string time1,
//			string time2,
//			string city,
//			string description,
//			TrainClass image,
//			string type = null,
//			string beforeDepartureTime = null,
//			string departureDate = null,
//			bool internetRegistration = false)
//		{
//			DateTime endTime;
//			DateTime startTime;
//			time2 = time2.Replace("<br />", " ");
//			startTime = DateTime.ParseExact(time1, Defines.Common.TimeFormat, CultureInfo.InvariantCulture);

//			endTime = time2.Length > 10
//				? DateTime.Parse(
//					time2.Length == 12 ? time2 : time2.Remove(time2.Length - 1),
//					new CultureInfo(_localizationService.GetString("Language")))
//				: DateTime.ParseExact(departureDate + ' ' + time2, Defines.Common.TimeFormat, CultureInfo.InvariantCulture);
//			return new Train
//			{
//				StartTime = startTime,
//				EndTime = endTime,
//				City = city.Replace(UnknownStr1, " - "),
//				BeforeDepartureTime = beforeDepartureTime ?? description.Replace(UnknownStr, " "),
//				Type = type,
//				Image = image,
//				OnTheWay = departureDate == null ? null : _localizationService.GetString("OnWay") + OnTheWay(startTime, endTime),
//				DepartureDate = departureDate,
//				InternetRegistration = internetRegistration
//			};
//		}

//		public static List<TrainClass> GetImagePath(List<Match> match)
//		{
//			return match.Select(x => x.Groups["type"].Value)
//				.Where(x => !string.IsNullOrEmpty(x)).Select(
//					type =>
//					{
//						if (type.Contains(_localizationService.GetString("International")))
//							return TrainClass.INTERNATIONAL;
//						if (type.Contains(_localizationService.GetString("Interregional")))
//							return type.Contains(_localizationService.GetString("Business"))
//								? TrainClass.INTERREGIONALBUSINESS
//								: TrainClass.INTERREGIONALECONOM;
//						if (type.Contains(_localizationService.GetString("Regional")))
//							return type.Contains(_localizationService.GetString("Business"))
//								? TrainClass.REGIONALBUSINESS
//								: TrainClass.REGIONALECONOM;
//						return TrainClass.CITY;
//					}).ToList();
//		}

//		public static List<bool> GetInternetRegistrationsInformations(IEnumerable<Match> match)
//		{
//			return match.Select(x => x.Groups["internetRegistration"].Value).Where(x => !string.IsNullOrEmpty(x)).Select(
//				m =>
//				{
//					if (m.Contains("item"))
//						return true;
//					return false;
//				}).ToList();
//		}

//		public static List<AdditionalInformation[]> GetPlaces(string data)
//		{
//			var additionInformation = new List<AdditionalInformation[]>();
//			var additionalParameter = data.ParseAsHtml(_localizationService.GetString("AdditionParameterPattern")).ToList();
//			for (var i = 0; i < additionalParameter.Count; i++)
//			{
//				if (i + 1 == additionalParameter.Count ||
//				    additionalParameter[i + 1].Groups[1].Value.Contains("href"))
//					additionInformation.Add(
//						new[]
//						{
//							new AdditionalInformation {Name = _localizationService.GetString("NoPlace")}
//						});
//				else
//				{
//					var temp =
//						additionalParameter[i + 1].Groups[1].Value.ParseAsHtml(_localizationService.GetString("PlacesAndPricesPattern"))
//							.ToList();

//					var additionalInformations = new AdditionalInformation[temp.Count/3];
//					for (var j = 0; j < temp.Count; j += 3)
//					{
//						additionalInformations[j/3] = new AdditionalInformation
//						{
//							Name = temp[j].Groups[1].Value.Length > 18
//								? _localizationService.GetString("Sedentary")
//								: temp[j].Groups[1].Value,
//							Place = _localizationService.GetString("Place") + (temp[j + 1].Groups[2].Value == UnknownStr
//								? _localizationService.GetString("Unlimited")
//								: temp[j + 1].Groups[2].Value.Replace(UnknownStr, string.Empty)),
//							Price = temp[j + 2].Groups[3].Value == string.Empty
//								? _localizationService.GetString("Unknown")
//								: _localizationService.GetString("Price") + temp[j + 2].Groups[3].Value.Replace(UnknownStr, " ")
//						};
//					}
//					additionInformation.Add(additionalInformations);
//					++i;
//				}
//			}
//			return additionInformation;
//		}

//		private static string GetBeforeDepartureTime(DateTime time, DateTime dateToDeparture)
//		{
//			if (dateToDeparture >= DateTime.Now)
//				return dateToDeparture.ToString("D");
//			var timeSpan = (time.TimeOfDay - DateTime.Now.TimeOfDay);
//			var hours = timeSpan.Hours == 0 ? string.Empty : (timeSpan.Hours + _localizationService.GetString("Hour"));
//			return _localizationService.GetString("Via") + hours + timeSpan.Minutes + _localizationService.GetString("Min");
//		}

//		private static string OnTheWay(DateTime startTime, DateTime endTime)
//		{
//			var time = endTime - startTime;
//			if (time.Days == 0)
//				return time.Hours + _localizationService.GetString("Hour") + time.Minutes + _localizationService.GetString("Min");
//			return (int) time.TotalHours + _localizationService.GetString("Hour") + time.Minutes +
//			       _localizationService.GetString("Min");
//		}

//		public static List<string> GetLink(string data)
//		{
//			var links = data.ParseAsHtml("<a href=\"/m/" + _localizationService.GetString("Language") + "/train/(.+?)\"");
//			return links.Select(x => x.Groups[1].Value).ToList();
//		}

//		public static IEnumerable<Train> GetFinallyResult(
//			IReadOnlyList<AdditionalInformation[]> additionalInformation,
//			IReadOnlyList<string> linksList,
//			IEnumerable<Train> trains)
//		{
//			var trainsList = trains.ToList();
//			for (var i = 0; i < additionalInformation.Count; i++)
//			{
//				trainsList[i].AdditionalInformation = additionalInformation[i];
//				trainsList[i].Link = linksList[i];

//				if (additionalInformation[i].Any())
//					if (trainsList[i].DepartureDate != null)
//						trainsList[i].IsPlace = additionalInformation[i].First().Name.Contains(_localizationService.GetString("No"))
//							? _localizationService.GetString("PlaceNo")
//							: _localizationService.GetString("PlaceYes");
//					else
//						trainsList[i].AdditionalInformation.First().Name = _localizationService.GetString("SpecifyDate");

//				var placeClasses = new PlaceClasses();
//				foreach (var name in additionalInformation[i].Select(x => x.Name))
//				{
//					if (_localizationService.GetString("Sedentary") == name)
//						placeClasses.IsSedentaryAvailable = true;
//					else if (_localizationService.GetString("SecondClass") == name)
//						placeClasses.IsSecondClassAvailable = true;
//					else if (_localizationService.GetString("Coupe") == name)
//						placeClasses.IsCoupeAvailable = true;
//					else if (_localizationService.GetString("Luxury") == name)
//						placeClasses.IsLuxuryAvailable = true;
//				}
//				trainsList[i].PlaceClasses = placeClasses;
//			}
//			return trainsList.Where(x => !x.BeforeDepartureTime.Contains("-"));
//		}

//		#endregion
//	}
//}