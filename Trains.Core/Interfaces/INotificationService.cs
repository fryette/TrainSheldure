using System;
using System.Threading.Tasks;
using Trains.Entities;

namespace Trains.Core.Interfaces
{
	public interface INotificationService
	{
		Task<TimeSpan> AddTrainToNotification(Train train, TimeSpan reminder);
	}
}
