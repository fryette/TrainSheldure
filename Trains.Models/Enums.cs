using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Trains.Models
{
	[JsonConverter(typeof(StringEnumConverter))]
	public enum TrainClass
    {
        International = 0,
        InterRegionalBusiness = 1,
        InterRegionalEconom = 2,
        RegionalBusiness = 3,
        RegionalEconom = 4,
        City = 5,
        Foreign = 6,
    }

	[JsonConverter(typeof(StringEnumConverter))]
	public enum Carriage
    {
        FirstClassSleeper = 0,
        CompartmentSleeper = 1,
        EconomyClassSleeper = 2,
        SeatingCoaches1 = 3,
        SeatingCoaches2 = 4,
        MultipleUnitCars1 = 5,
        MultipleUnitCars2 = 6,
        MultipleUnitCoach = 7
    }

	[JsonConverter(typeof(StringEnumConverter))]
	public enum AboutPicture
    {
        Market = 0,
        Settings = 1,
        Mail = 2,
        AboutApp = 3,
        Share=4
    }

	[JsonConverter(typeof(StringEnumConverter))]
	public enum Seats
    {
		Sedentary = 0,
		SecondClass = 1,
		Coupe = 2,
		Luxury = 3,
    }

	[JsonConverter(typeof(StringEnumConverter))]
	public enum ShareSocial
    {
        Vkontakte = 0,
        Facebook = 1,
        Twitter = 2,
        LinkedIn = 3,
        GooglePlus = 4,
        Odnoklassniki = 5
    }
}
