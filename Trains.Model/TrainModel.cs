using Trains.Model.Entities;

namespace Trains.Model
{
	public class TrainModel
	{
		public TrainTimeModel Time { get; set; }
		public TrainInformationModel Information { get; set; }
		public string StopPointsUrl { get; set; }
		public PlaceClasses Clases { get; set; }
		public TrainClass Type { get; set; }
		public bool IsElectronicRegistrationAvailable { get; set; }
	}
}