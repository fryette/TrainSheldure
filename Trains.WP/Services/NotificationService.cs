using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Appointments;
using Trains.Infrastructure;
using Trains.Infrastructure.Interfaces.Platform;
using Trains.Model;
using Trains.Model.Entities;

namespace Trains.WP.Services
{
	public class NotificationService : INotificationService
	{
		private AppointmentCalendar _currentAppCalendar;

		public async Task<TimeSpan> AddTrainToNotification(TrainModel train, TimeSpan reminder)
		{
			if (_currentAppCalendar == null)
				await CheckForAndCreateAppointmentCalendars();

			var tempTime = train.Time.StartTime - DateTime.Now;
			var timeToNotify = tempTime < reminder ? new TimeSpan(tempTime.Ticks / 2) : reminder;

			var newAppointment = new Appointment
			{
				Subject = train.Information.Name,
				StartTime = train.Time.StartTime,
				Duration = train.Time.EndTime - train.Time.StartTime,
				Reminder = timeToNotify,
				Location = train.Information.Name,
				RoamingId = train.Information.Name
			};

			await _currentAppCalendar.SaveAppointmentAsync(newAppointment);

			return timeToNotify;
		}

		public async Task CheckForAndCreateAppointmentCalendars()
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
