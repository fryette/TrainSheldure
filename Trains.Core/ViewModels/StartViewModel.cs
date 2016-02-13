using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Chance.MvvmCross.Plugins.UserInteraction;
using Cirrious.MvvmCross.ViewModels;
using Trains.Entities;
using Trains.Infrastructure;
using Trains.Infrastructure.Extensions;
using Trains.Infrastructure.Interfaces;
using Trains.Infrastructure.Interfaces.Platform;
using Trains.Infrastructure.Interfaces.Services;
using Trains.Model.Entities;

namespace Trains.Core.ViewModels
{
	public class StartViewModel : MvxViewModel
	{
		private readonly IAppSettings _appSettings;
		private readonly ISerializableService _serializable;
		private readonly IUserInteraction _userInteraction;
		private readonly ILocalDataService _local;
		private readonly ILocalizationService _localizationService;

		public ICommand LoadedCommand { get; set; }

		public StartViewModel(IAppSettings appSettings,
			IUserInteraction userInteraction,
			ISerializableService serializable,
			ILocalDataService local, ILocalizationService localizationService)
		{
			_appSettings = appSettings;
			_userInteraction = userInteraction;
			_serializable = serializable;
			_local = local;
			_localizationService = localizationService;

			LoadedCommand = new MvxCommand(RestoreData);
		}

		private void RestoreData()
		{
			if (_localizationService.CurrentLanguageId == null)
			{
				_serializable.ClearAll();

				ShowViewModel<CountrySelectionViewModel>();
				return;
			}

			var appSettings = _serializable.Desserialize<AppSettings>(Defines.Restoring.AppSettings);
			if (appSettings == null) return;

			appSettings.CopyProperties(_appSettings);

			_appSettings.UpdatedLastRequest = _serializable.Desserialize<LastRequest>(Defines.Restoring.UpdateLastRequest);
			_appSettings.LastRequestTrain = _serializable.Desserialize<List<Train>>(Defines.Restoring.LastTrainList);
			_appSettings.Tickets = appSettings.Tickets;

			var routes = _serializable.Desserialize<List<Route>>(Defines.Restoring.LastRoutes);

			_appSettings.LastRoutes = routes ?? new List<Route>();

			ShowViewModel<MainViewModel>();
		}
	}
}
