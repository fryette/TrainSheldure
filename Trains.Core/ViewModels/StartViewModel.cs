using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Chance.MvvmCross.Plugins.UserInteraction;
using Cirrious.MvvmCross.ViewModels;
using Trains.Infrastructure;
using Trains.Infrastructure.Extensions;
using Trains.Infrastructure.Interfaces;
using Trains.Infrastructure.Interfaces.Platform;
using Trains.Infrastructure.Interfaces.Services;
using Trains.Model;
using Trains.Model.Entities;

namespace Trains.Core.ViewModels
{
	public class StartViewModel : MvxViewModel
	{
		private readonly IAppSettings _appSettings;
		private readonly ISorageProvider _sorage;
		private readonly IUserInteraction _userInteraction;
		private readonly ILocalDataService _local;
		private readonly ILocalizationService _localizationService;

		public ICommand LoadedCommand { get; set; }

		public StartViewModel(IAppSettings appSettings,
			IUserInteraction userInteraction,
			ISorageProvider sorage,
			ILocalDataService local, ILocalizationService localizationService)
		{
			_appSettings = appSettings;
			_userInteraction = userInteraction;
			_sorage = sorage;
			_local = local;
			_localizationService = localizationService;

			LoadedCommand = new MvxCommand(RestoreData);
		}

		private void RestoreData()
		{
			if (_localizationService.CurrentLanguageId == null)
			{
				_sorage.ClearAll();

				ShowViewModel<CountrySelectionViewModel>();
				return;
			}
			try
			{
				var appSettings = _sorage.ReadAndMap<AppSettings>(Defines.Restoring.AppSettings);

				if (appSettings == null)
				{
					return;
				}

				appSettings.CopyProperties(_appSettings);

				_appSettings.UpdatedLastRequest = _sorage.ReadAndMap<LastRequest>(Defines.Restoring.UpdateLastRequest);
				_appSettings.LastRequestedTrains = _sorage.ReadAndMap<List<TrainModel>>(Defines.Restoring.LastTrainList);

				var routes = _sorage.ReadAndMap<List<Route>>(Defines.Restoring.LastRoutes);
				_appSettings.LastRoutes = routes ?? new List<Route>();

			}
			catch
			{
				_appSettings.LastRoutes = new List<Route>();
			}

			ShowViewModel<MainViewModel>();
		}
	}
}
