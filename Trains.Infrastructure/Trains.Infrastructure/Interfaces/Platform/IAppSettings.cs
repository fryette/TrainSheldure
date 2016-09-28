using System;
using System.Collections.Generic;
using Trains.Model;
using Trains.Model.Entities;

namespace Trains.Infrastructure.Interfaces.Platform
{
	public interface IAppSettings
	{
		List<CountryStopPointItem> AutoCompletion { get; set; }

		LastRequest UpdatedLastRequest { get; set; }

		IEnumerable<HelpInformationItem> HelpInformation { get; set; }

		IEnumerable<TrainModel> LastRequestTrain { get; set; }

		IEnumerable<CarriageModel> CarriageModel { get; set; }

		IEnumerable<About> About { get; set; }

		IEnumerable<Route> LastRoutes { get; set; }

		bool FirstUpdateRun { get; set; }

		SocialUri SocialUri { get; set; }

		IEnumerable<PlaceInformation> PlaceInformation { get; set; }

		List<Country> Countries { get; set; }

		TimeSpan Reminder { get; set; }

		PersonalInformation PersonalInformation { get; set; }

	}
}
