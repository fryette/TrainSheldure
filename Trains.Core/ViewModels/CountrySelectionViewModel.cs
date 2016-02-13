using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using Chance.MvvmCross.Plugins.UserInteraction;
using Cirrious.MvvmCross.ViewModels;
using Trains.Infrastructure;
using Trains.Infrastructure.Interfaces;
using Trains.Infrastructure.Interfaces.Platform;
using Trains.Infrastructure.Interfaces.Providers;
using Trains.Infrastructure.Interfaces.Services;
using Trains.Model.Entities;

namespace Trains.Core.ViewModels
{
	public class CountrySelectionViewModel : MvxViewModel
	{
		private const string Countries = "countries";
		public ICommand NavigateToNextViewCommand { get; set; }

		public List<Language> Languages { get; private set; }
		private Language _selectedLanguage;
		private readonly ISerializableService _serializable;
		private readonly IAnalytics _analytics;
		private readonly IConfigurationProvider _configurationProvider;
		private readonly IUserInteraction _userInteraction;
		private readonly IAppSettings _appSettings;
		private readonly ILocalDataService _local;
		private readonly ILocalizationService _localizationService;

		public CountrySelectionViewModel(IAnalytics analytics,
			ISerializableService serializable,
			IConfigurationProvider configurationProvider,
			IUserInteraction userInteraction,
			IAppSettings appSettings,
			ILocalDataService local, ILocalizationService localizationService)
		{
			_analytics = analytics;
			_serializable = serializable;
			_configurationProvider = configurationProvider;
			_userInteraction = userInteraction;
			_appSettings = appSettings;
			_local = local;
			_localizationService = localizationService;

			NavigateToNextViewCommand = new MvxCommand(SaveChangesAsync);
		}

		public void Init()
		{
			Languages = _configurationProvider.Load<List<Language>>(Countries);
			_selectedLanguage = Languages.First();
		}

		private bool _isDownloadRunning;
		public bool IsDownloadRunning
		{
			get { return _isDownloadRunning; }
			set
			{
				_isDownloadRunning = value;
				RaisePropertyChanged(() => IsDownloadRunning);
			}
		}
		public Language SelectedLanguage
		{
			get
			{
				return _selectedLanguage;
			}
			set
			{
				_selectedLanguage = value;
				RaisePropertyChanged(() => SelectedLanguage);
			}
		}

		private async void SaveChangesAsync()
		{
			_analytics.SentEvent(Defines.Analytics.LanguageChanged, SelectedLanguage.Name);
			_serializable.Serialize(SelectedLanguage, Defines.Restoring.AppLanguage);
			_localizationService.ChangeLocale(SelectedLanguage.Id);

			await _userInteraction.AlertAsync(Defines.Common.HiMessage, Defines.Common.HiMessageTitle);
			await DowloadResources();
			ShowViewModel<MainViewModel>();
		}

		private async Task DowloadResources()
		{
			IsDownloadRunning = true;

			_appSettings.AutoCompletion = await _local.GetLanguageData<List<CountryStopPointItem>>(Defines.DownloadJson.StopPoints);
			_appSettings.HelpInformation = await _local.GetLanguageData<List<HelpInformationItem>>(Defines.DownloadJson.HelpInformation);
			_appSettings.CarriageModel = await _local.GetLanguageData<List<CarriageModel>>(Defines.DownloadJson.CarriageModel);
			_appSettings.About = await _local.GetLanguageData<List<About>>(Defines.DownloadJson.About);
			_appSettings.SocialUri = await _local.GetOtherData<SocialUri>(Defines.DownloadJson.Social);
			_appSettings.Tickets = await _local.GetOtherData<List<Ticket>>(Defines.DownloadJson.Tickets);
			_appSettings.PlaceInformation = await _local.GetLanguageData<List<PlaceInformation>>(Defines.DownloadJson.PlaceInformation);
			_appSettings.Countries = await _local.GetLanguageData<List<Country>>(Defines.DownloadJson.Countries);

			_serializable.Serialize(await _local.GetLanguageData<Dictionary<string, string>>(Defines.DownloadJson.Resource), Defines.Restoring.ResourceLoader);

			if (_appSettings.Reminder.Seconds == 0)
			{
				_appSettings.Reminder = new TimeSpan(1, 0, 0);
			}

			_serializable.Serialize(_appSettings, Defines.Restoring.AppSettings);

			IsDownloadRunning = true;
		}
	}
}