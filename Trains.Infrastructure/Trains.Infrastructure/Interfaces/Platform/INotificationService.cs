using System;
using System.Threading.Tasks;
using Trains.Entities;

namespace Trains.Infrastructure.Interfaces.Platform
{
	public interface INotificationService
	{
		Task<TimeSpan> AddTrainToNotification(Train train, TimeSpan reminder);
	}
}
