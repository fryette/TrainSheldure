using System;
using System.Collections.Generic;
using Trains.Infrastructure.Interfaces;
using Trains.Models;

namespace Trains.Infrastructure
{
	public class AppSettings : IAppSettings
	{
		public List<LastRequest> FavoriteRequests { get; set; }
		public LastRequest UpdatedLastRequest { get; set; }
		public List<Train> LastRequestTrain { get; set; }

		public List<CountryStopPointItem> AutoCompletion { get; set; }
		public List<PlaceInformation> PlaceInformation { get; set; }
		public List<Country> Countries { get; set; }
		public List<HelpInformationItem> HelpInformation { get; set; }
		public List<CarriageModel> CarriageModel { get; set; }
		public List<About> About { get; set; }

		public bool FirstUpdateRun { get; set; }
		public List<Route> LastRoutes { get; set; }

		public SocialUri SocialUri { get; set; }
		public Language Language { get; set; }
		public TimeSpan Reminder { get; set; }
	    public List<Ticket> Tickets { get; set; }
	    public PersonalInformation PersonalInformation { get; set; }
	}
}
