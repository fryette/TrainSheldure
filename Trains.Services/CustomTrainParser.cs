using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using HtmlAgilityPack;
using Trains.Infrastructure.Interfaces;
using Trains.Model;
using Trains.Model.Entities;

namespace Trains.Services
{
	public class CustomTrainParser
	{
		private const string CLASS_ATTRIBUTE = "class";
		private const string TIME_FORMAT = "HH:mm";

		private readonly ILocalizationService _localizationService;

		public CustomTrainParser(ILocalizationService localizationService)
		{
			_localizationService = localizationService;
		}

		public IEnumerable<TrainModel> TestMethod(string data)
		{
			var htmlDoc = new HtmlDocument { OptionFixNestedTags = true };

			// There are various options, set as needed

			// filePath is a path to a file containing the html
			htmlDoc.LoadHtml(data);

			// ParseErrors is an ArrayList containing any errors from the Load statement
			if (htmlDoc.ParseErrors != null && htmlDoc.ParseErrors.Any())
			{
				// Handle any parse errors as required
			}
			else
			{
				if (htmlDoc.DocumentNode != null)
				{
					return ParseTrainInformation(htmlDoc.DocumentNode);
				}
			}

			return Enumerable.Empty<TrainModel>();
		}

		private IEnumerable<HtmlNode> LoadDescendantsAsContains(HtmlNode node, string attribute, string attributeName)
		{
			return
				node.Descendants().Where(n => n.GetAttributeValue(attribute, string.Empty).Contains(attributeName));
		}

		private IEnumerable<HtmlNode> LoadDescendantsAsEquals(HtmlNode node, string attribute, string attributeName)
		{
			return
				node.Descendants().Where(n => n.GetAttributeValue(attribute, string.Empty).Equals(attributeName));
		}

		private IEnumerable<TrainModel> ParseTrainInformation(HtmlNode documentNode)
		{
			var trainsInformationNode = LoadDescendantsAsContains(documentNode, CLASS_ATTRIBUTE, "list_items").FirstOrDefault();
			var placesNode = LoadDescendantsAsContains(documentNode, CLASS_ATTRIBUTE, "b-places").ToList();

			if (trainsInformationNode == null)
			{
				return Enumerable.Empty<TrainModel>();
			}

			var trainsNodeList = LoadDescendantsAsContains(trainsInformationNode, CLASS_ATTRIBUTE, "list_item").ToList();

			var result = new List<TrainModel>();

			for (var i = 0; i < trainsNodeList.Count; i++)
			{
				var train = CreateTrainModel(trainsNodeList[i]);
				FillPlaceInformation(train, ref placesNode);
				result.Add(train);
			}

			return result;
		}

		private TrainModel CreateTrainModel(HtmlNode htmlNode)
		{
			return new TrainModel
			{
				Time = ParseTime(htmlNode),
				Type = ParseTrainType(htmlNode),
				Information = ParseInformation(htmlNode)
			};
		}

		private TrainModel FillPlaceInformation(TrainModel model, ref List<HtmlNode> placeNodes)
		{
			model.StopPointsUrl = ParseUrl(placeNodes.First());
			placeNodes.RemoveAt(0);

			model.Clases = ParseClases(placeNodes.FirstOrDefault());

			if (model.Clases == null)
			{
				model.Clases = new PlaceClasses();
			}
			else
			{
				placeNodes.RemoveAt(0);
			}

			return model;
		}

		private PlaceClasses ParseClases(HtmlNode htmlNode)
		{
			if (htmlNode == null)
			{
				return null;
			}

			var placeResults =
				LoadDescendantsAsEquals(htmlNode, CLASS_ATTRIBUTE, "places_name").Select(x => x.InnerText).ToList();
			var availablePlaceCount =
				LoadDescendantsAsEquals(htmlNode, CLASS_ATTRIBUTE, "places_qty").Select(x => x.InnerText).ToList();
			var priceResults =
				LoadDescendantsAsEquals(htmlNode, CLASS_ATTRIBUTE, "denom_after").Select(x => x.InnerText).ToList();

			if (!placeResults.Any())
			{
				return null;
			}

			return ParseTrainPlaces(placeResults, availablePlaceCount, priceResults);
		}

		private PlaceClasses ParseTrainPlaces(
			IReadOnlyList<string> placeResults,
			IReadOnlyList<string> placeCountsResult,
			IReadOnlyList<string> priceResults)
		{
			var placeClasses = new PlaceClasses();

			for (var i = 0; i < placeResults.Count; i++)
			{
				if (_localizationService.GetString("General").Equals(placeResults[i]))
				{
					placeClasses.IsGeneralAvailable = true;
					placeClasses.GeneralCount = placeCountsResult[i];
					placeClasses.GeneralPrice = priceResults[i];
				}
				if (_localizationService.GetString("Sedentary").Equals(placeResults[i]))
				{
					placeClasses.IsSedentaryAvailable = true;
					placeClasses.SedentaryCount = placeCountsResult[i];
					placeClasses.SedentaryPrice = priceResults[i];
				}
				else if (_localizationService.GetString("SecondClass").Equals(placeResults[i]))
				{
					placeClasses.IsSecondClassAvailable = true;
					placeClasses.SecondClassCount = placeCountsResult[i];
					placeClasses.SecondClassPrice = priceResults[i];
				}
				else if (_localizationService.GetString("Coupe").Equals(placeResults[i]))
				{
					placeClasses.IsCoupeAvailable = true;
					placeClasses.CoupeCount = placeCountsResult[i];
					placeClasses.CoupePrice = priceResults[i];
				}
				else if (_localizationService.GetString("Luxury").Equals(placeResults[i]))
				{
					placeClasses.IsLuxuryAvailable = true;
					placeClasses.LuxuryCount = placeCountsResult[i];
					placeClasses.LuxuryPrice = priceResults[i];
				}
			}

			return placeClasses;
		}

		private string ParseUrl(HtmlNode htmlNode)
		{
			var urlNode = LoadDescendantsAsContains(htmlNode, CLASS_ATTRIBUTE, "list_item").FirstOrDefault();

			return urlNode?.Attributes.FirstOrDefault()?.Value;
		}

		private TrainTimeModel ParseTime(HtmlNode htmlNode)
		{
			var timesNode = LoadDescendantsAsContains(htmlNode, CLASS_ATTRIBUTE, "list_time").FirstOrDefault();

			if (timesNode == null)
			{
				return null;
			}

			return new TrainTimeModel
			{
				StartTime =
					ConvertStringToDateTime(
						LoadDescendantsAsContains(timesNode, CLASS_ATTRIBUTE, "list_start").FirstOrDefault()?.InnerText),
				EndTime =
					ConvertStringToDateTime(
						LoadDescendantsAsContains(timesNode, CLASS_ATTRIBUTE, "list_end").FirstOrDefault()?.InnerText)
			};
		}

		private DateTime ConvertStringToDateTime(string time)
		{
			if (string.IsNullOrEmpty(time))
			{
				return DateTime.Now;
			}

			if (time.Length > 10)
			{
				time = time.Insert(5, " ");

				return DateTime.Parse(
					time.Length == 12 ? time : time.Remove(time.Length - 1),
					new CultureInfo(_localizationService.GetString("Language")));
			}

			return DateTime.ParseExact(time, TIME_FORMAT, CultureInfo.InvariantCulture);
		}

		private TrainInformationModel ParseInformation(HtmlNode htmlNode)
		{
			var trainContent = LoadDescendantsAsContains(htmlNode, CLASS_ATTRIBUTE, "list_content").FirstOrDefault();

			if (trainContent == null)
			{
				return null;
			}

			var trainNameNode = LoadDescendantsAsContains(trainContent, CLASS_ATTRIBUTE, "list_text").FirstOrDefault();
			var trainSchedule = LoadDescendantsAsContains(trainContent, CLASS_ATTRIBUTE, "list_text_small").FirstOrDefault();

			return new TrainInformationModel
			{
				Name = System.Net.WebUtility.HtmlDecode(trainNameNode.InnerText),
				Schedule = trainSchedule.InnerText
			};
		}

		private TrainClass ParseTrainType(HtmlNode htmlNode)
		{
			var imageNode = LoadDescendantsAsContains(htmlNode, CLASS_ATTRIBUTE, "b-pic pic").FirstOrDefault();

			if (imageNode == null)
			{
				return TrainClass.DEFAULT;
			}

			return GetImageType(imageNode.OuterHtml);
		}

		private TrainClass GetImageType(string imageType)
		{
			if (imageType.Contains("international"))
				return TrainClass.INTERNATIONAL;
			if (imageType.Contains("interregional"))
				return imageType.Contains("business")
					? TrainClass.INTERREGIONALBUSINESS
					: TrainClass.INTERREGIONALECONOM;
			if (imageType.Contains("regional"))
				return imageType.Contains("business")
					? TrainClass.REGIONALBUSINESS
					: TrainClass.REGIONALECONOM;
			return TrainClass.CITY;
		}
	}
}