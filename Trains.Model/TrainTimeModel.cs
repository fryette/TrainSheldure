using System;

namespace Trains.Model
{
	public class TrainTimeModel
	{
		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }
		public TimeSpan OnTheWay => EndTime - StartTime;
	}
}