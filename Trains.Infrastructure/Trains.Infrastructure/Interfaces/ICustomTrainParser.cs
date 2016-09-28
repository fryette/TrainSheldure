using System.Collections.Generic;
using Trains.Model;

namespace Trains.Services
{
	public interface ICustomTrainParser
	{
		IEnumerable<TrainModel> LoadDataOnTheDay(string data);
	}
}