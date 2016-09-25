using System;
using System.Threading.Tasks;
using Trains.Model;

namespace Trains.Infrastructure.Interfaces.Platform
{
	public interface INotificationService
	{
		Task<TimeSpan> AddTrainToNotification(TrainModel train, TimeSpan reminder);
	}
}
