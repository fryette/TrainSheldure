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

		//images and Internet Registrations
		private const int SearchCountParameter = 2;

		#endregion

		#region action

		public static List<Train> GetTrainsInformation(List<Match> parameters, string date, List<bool> isInternetRegistration)
		{
			var imagePath = new List<TrainClass>(GetImagePath(parameters));
			var trainList = new List<Train>(parameters.Count / SearchCountParameter);
			var step = parameters.Count - imagePath.Count * 2;

			for (var i = 0; i < step; i += 4)
			{
				trainList.Add(
					CreateTrain(
					date + ' ' + parameters[i].Groups[1].Value,
					parameters[i + 1].Groups[2].Value,
					parameters[i + 2].Groups[3].Value,
					imagePath[i / 4],
					parameters[i / 4 + step].Value,
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
					parameters[i + 2].Groups[3].Value, TrainClass.Foreign, null, date));
			}
			return trainList;
		}

		private static Train CreateTrain(string time1, string time2, string city, TrainClass image, string type = null, string departureDate = null, bool internetRegistration = false)
		{
			time2 = time2.Replace("<br />", " ");
			var startTime = DateTime.ParseExact(time1, Defines.Common.TimeFormat, CultureInfo.InvariantCulture);

			var endTime = time2.Length > 10 ? DateTime.Parse(time2.Length == 12 ? time2 : time2.Remove(time2.Length - 1), new CultureInfo(ResourceLoader.Instance.Resource["Language"]))
				: DateTime.ParseExact(departureDate + ' ' + time2, Defines.Common.TimeFormat, CultureInfo.InvariantCulture);
			return new Train
			{
				StartTime = startTime,
				EndTime = endTime,
				City = city,
				Type = GetTrainType(type),
				Image = image,
				DepartureDate = departureDate,
				InternetRegistration = internetRegistration
			};
		}

		public static List<TrainClass> GetImagePath(List<Match> match)
		{
			return match.Select(x => x.Groups["type"].Value)
				.Where(x => !string.IsNullOrEmpty(x)).Select(GetTrainType).ToList();
		}

		private static TrainClass GetTrainType(string type)
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

		private static Seats GetSeatType(string type)
		{
			if (type.Contains(ResourceLoader.Instance.Resource["Luxury"]))
				return Seats.Luxury;
			if (type.Contains(ResourceLoader.Instance.Resource["SecondClass"]))
				return Seats.SecondClass;
			if (type.Contains(ResourceLoader.Instance.Resource["Coupe"]))
				return Seats.Coupe;
			return Seats.Sedentary;
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
				if (i + 1 == additionalParameter.Count || additionalParameter[i + 1].Groups[1].Value.Contains("href")) continue;
				var temp =
					Parser.ParseData(additionalParameter[i + 1].Groups[1].Value, Defines.Common.PlacesAndPricesPattern)
						.ToList();
				var additionalInformations = new AdditionalInformation[temp.Count/3];
				for (var j = 0; j < temp.Count; j += 3)
				{
					additionalInformations[j/3] = new AdditionalInformation
					{
						Name = GetSeatType(temp[j].Groups[1].Value),
						Place = temp[j + 1].Groups[2].Value.Trim(),
						Price = temp[j + 2].Groups[3].Value
					};
				}
				additionInformation.Add(additionalInformations);
				++i;
			}
			return additionInformation;
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
			}

			return trainsList;
		}
		#endregion
	}
}