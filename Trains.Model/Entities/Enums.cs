﻿namespace Trains.Model.Entities
{
	public enum TrainClass
	{
		INTERNATIONAL = 0,
		INTER_REGIONAL_BUSINESS = 1,
		INTER_REGIONAL_ECONOM = 2,
		REGIONAL_BUSINESS = 3,
		REGIONAL_ECONOM = 4,
		CITY = 5,
		FOREIGN = 6,
	}

	public enum Carriage
	{
		FIRST_CLASS_SLEEPER = 0,
		COMPARTMENT_SLEEPER = 1,
		ECONOMY_CLASS_SLEEPER = 2,
		SEATING_COACHES1 = 3,
		SEATING_COACHES2 = 4,
		MULTIPLE_UNIT_CARS1 = 5,
		MULTIPLE_UNIT_CARS2 = 6,
		MULTIPLE_UNIT_COACH = 7
	}

	public enum AboutPicture
	{
		MARKET = 0,
		SETTINGS = 1,
		MAIL = 2,
		ABOUT_APP = 3,
		SHARE = 4
	}
	public enum ShareSocial
	{
		VKONTAKTE = 0,
		FACEBOOK = 1,
		TWITTER = 2,
		LINKED_IN = 3,
		GOOGLE_PLUS = 4,
		ODNOKLASSNIKI = 5
	}
}
