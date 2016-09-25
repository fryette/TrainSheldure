using System;
using System.Collections.Generic;
using Trains.Infrastructure.Interfaces.Platform;
using Trains.Model;
using Trains.Model.Entities;

namespace Trains.Core
{
	public class AppSettings : IAppSettings
	{
		public List<LastRequest> FavoriteRequests { get; set; }
		public LastRequest UpdatedLastRequest { get; set; }
		public IEnumerable<TrainModel> LastRequestTrain { get; set; }

		public List<CountryStopPointItem> AutoCompletion { get; set; }
		public IEnumerable<PlaceInformation> PlaceInformation { get; set; }
		public List<Country> Countries { get; set; }
		public IEnumerable<HelpInformationItem> HelpInformation { get; set; }
		public IEnumerable<CarriageModel> CarriageModel { get; set; }
		public IEnumerable<About> About { get; set; }

		public bool FirstUpdateRun { get; set; }
		public IEnumerable<Route> LastRoutes { get; set; }

		public SocialUri SocialUri { get; set; }
		public Language Language { get; set; }
		public TimeSpan Reminder { get; set; }
		public IEnumerable<Ticket> Tickets { get; set; }
		public PersonalInformation PersonalInformation { get; set; }
	}
}
