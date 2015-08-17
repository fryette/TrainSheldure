using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Appointments;
using Trains.Core.Interfaces;
using Trains.Core.Resources;
using Trains.Entities;

namespace Trains.WP.Services
{
	public class NotificationService : INotificationService
	{
		private AppointmentCalendar _currentAppCalendar;

		public async Task<TimeSpan> AddTrainToNotification(Train train, TimeSpan reminder)
		{
			if (_currentAppCalendar == null)
				await CheckForAndCreateAppointmentCalendars();

			var tempTime = train.StartTime - DateTime.Now;
			var timeToNotify = tempTime < reminder ? new TimeSpan(tempTime.Ticks / 2) : reminder;

			var newAppointment = new Appointment
			{
				Subject = train.City,
				StartTime = train.StartTime,
				Duration = train.EndTime - train.StartTime,
				Reminder = timeToNotify,
				Location = train.City,
				RoamingId = train.City
			};

			await _currentAppCalendar.SaveAppointmentAsync(newAppointment);

			return timeToNotify;
		}

		async public Task CheckForAndCreateAppointmentCalendars()
		{
			var appointmentStore = await AppointmentManager.RequestStoreAsync(AppointmentStoreAccessType.AppCalendarsReadWrite);
			var appCalendars =
				await appointmentStore.FindAppointmentCalendarsAsync(FindAppointmentCalendarsOptions.IncludeHidden);

			AppointmentCalendar appCalendar = null;

			if (appCalendars.Count == 0)
			{
				appCalendar = await appointmentStore.CreateAppointmentCalendarAsync(Defines.Common.Name);
			}
			else
			{
				appCalendar = appCalendars[0];
			}

			appCalendar.OtherAppReadAccess = AppointmentCalendarOtherAppReadAccess.Full;
			appCalendar.OtherAppWriteAccess = AppointmentCalendarOtherAppWriteAccess.SystemOnly;

			appCalendar.SummaryCardView = AppointmentSummaryCardView.App;

			await appCalendar.SaveAsync();

			_currentAppCalendar = appCalendar;
		}
	}
}
