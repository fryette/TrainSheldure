using System;
using System.Threading.Tasks;
using Trains.Models;

namespace Trains.Infrastructure.Interfaces
{
	public interface INotificationService
	{
		Task<TimeSpan> AddTrainToNotification(Train train, TimeSpan reminder);
	}
}
