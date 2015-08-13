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

		public async Task AddTrainToNotification(Train train, TimeSpan reminder)
		{
			if (_currentAppCalendar == null)
			{
				var appointmentStore = await AppointmentManager.RequestStoreAsync(AppointmentStoreAccessType.AppCalendarsReadWrite);
				_currentAppCalendar = (await appointmentStore.FindAppointmentCalendarsAsync(FindAppointmentCalendarsOptions.IncludeHidden))[0];
			}
			var newAppointment = new Appointment
			{
				Subject = train.City,
				StartTime = train.StartTime,
				Duration = train.EndTime - train.StartTime,
				Reminder = reminder.Minutes == 0 ? TimeSpan.FromHours(1) : reminder,
				Location = train.City,
				RoamingId = train.City
			};

			await _currentAppCalendar.SaveAppointmentAsync(newAppointment);
		}
	}
}
