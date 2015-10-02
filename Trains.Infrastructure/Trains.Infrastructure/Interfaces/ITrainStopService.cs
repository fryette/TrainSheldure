using System.Collections.Generic;
using System.Threading.Tasks;
using Trains.Models;

namespace Trains.Infrastructure.Interfaces
{
	public interface ITrainStopService
	{
		Task<IEnumerable<TrainStop>> GetTrainStop(string link);
	}
}
