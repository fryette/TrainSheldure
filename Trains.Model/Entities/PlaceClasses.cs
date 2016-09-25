namespace Trains.Model.Entities
{
	public class PlaceClasses
	{
		//общий
		public bool IsGeneralAvailable { get; set; }
		public string GeneralCount { get; set; }
		public string GeneralPrice { get; set; }
		//сидячий
		public bool IsSedentaryAvailable { get; set; }
		public string SedentaryCount { get; set; }
		public string SedentaryPrice { get; set; }
		//плацкартный
		public bool IsSecondClassAvailable { get; set; }
		public string SecondClassCount { get; set; }
		public string SecondClassPrice { get; set; }
		//купе
		public bool IsCoupeAvailable { get; set; }
		public string CoupeCount { get; set; }
		public string CoupePrice { get; set; }
		//СВ
		public bool IsLuxuryAvailable { get; set; }
		public string LuxuryCount { get; set; }
		public string LuxuryPrice { get; set; }
	}
}
