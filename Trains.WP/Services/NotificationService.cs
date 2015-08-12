using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Appointments;
using Trains.Core.Interfaces;
using Trains.Entities;

namespace Trains.WP.Services
{
	public class NotificationService : INotificationService
	{
		private AppointmentCalendar _currentAppCalendar;

		public async Task AddTrainToNotification(Train train)
		{
			//if (_currentAppCalendar == null)
			//{
			//	var appointmentStore = await AppointmentManager.RequestStoreAsync(AppointmentStoreAccessType.AppCalendarsReadWrite);
			//	_currentAppCalendar = (await appointmentStore.FindAppointmentCalendarsAsync(FindAppointmentCalendarsOptions.IncludeHidden))[0];
			//}

			//var newAppointment = new Appointment
			//{
			//	Subject = train.City,
			//	StartTime = DateTimeOffset.Parse(train.StartTime),
			//	Duration = TimeSpan.FromSeconds(10),
			//	Reminder = TimeSpan.FromMinutes(10),
			//	Location = "Minsk",
			//	RoamingId = "TestId"
			//};
			
			////save appointment to calendar
			//await _currentAppCalendar.SaveAppointmentAsync(newAppointment);
		}
	}
}
