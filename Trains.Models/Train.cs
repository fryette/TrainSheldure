using System;

namespace Trains.Models
{
	public class Train
	{
		public string City { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }
		public TrainClass Image { get; set; }
		public TrainClass Type { get; set; }
		public AdditionalInformation[] AdditionalInformation { get; set; }
		public string DepartureDate { get; set; }
		public string Link { get; set; }
		public bool IsPlace { get; set; }
		public bool InternetRegistration { get; set; }

	}

	public class AdditionalInformation
	{
		public Seats Name { get; set; }
		public string Price { get; set; }
		public string Place { get; set; }
	}
}