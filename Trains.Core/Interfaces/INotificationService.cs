using System.Threading.Tasks;
using Trains.Entities;

namespace Trains.Core.Interfaces
{
	public interface INotificationService
	{
		Task AddTrainToNotification(Train train);
	}
}
