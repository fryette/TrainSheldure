using System;
using System.Collections.Generic;
using System.Linq;
using Chance.MvvmCross.Plugins.UserInteraction;
using Cirrious.MvvmCross.ViewModels;
using Trains.Core.Interfaces;
using Trains.Core.Resources;
using Trains.Core.Services.Interfaces;
using Trains.Model.Entities;
using static System.String;
using static Trains.Core.Resources.Defines;

namespace Trains.Core.ViewModels
{
	public class SettingsViewModel : MvxViewModel
	{
		#region readonlyProperties

		private readonly ISerializableService _serializable;
		private readonly IAppSettings _appSettings;
		private readonly IAnalytics _analytics;
		private readonly ILocalDataService _local;
		private readonly IUserInteraction _userInteraction;
		private readonly ILocalizationService _localizationService;

		#endregion

		#region command

		public IMvxCommand ResetSettingsCommand { get; private set; }
		public IMvxCommand DownloadSelectedCountryStopPointsCommand { get; private set; }

		#endregion

		#region ctor

		public SettingsViewModel(ISerializableService serializable,
			IAppSettings appSettings,
			IAnalytics analytics,
			ILocalDataService local,
			IUserInteraction userInteraction,
			ILocalizationService localizationService)
		{
			_analytics = analytics;
			_serializable = serializable;
			_appSettings = appSettings;
			_local = local;
			_userInteraction = userInteraction;
			_localizationService = localizationService;

			ResetSettingsCommand = new MvxCommand(ResetSetting);
			DownloadSelectedCountryStopPointsCommand = new MvxCommand(DownloadCountryStopPoint);
		}

		#endregion

		#region properties

		#region UIproperties

		public string Header { get; set; }
		public string SelectCountries { get; set; }
		public string DownloadSelectCountry { get; set; }
		public string SelectLanguage { get; set; }
		public string ResetSettings { get; set; }
		public string TimeNotification { get; set; }

		private string _needReboot;
		public string NeedReboot
		{
			get
			{
				return _needReboot;
			}
			set
			{
				_needReboot = value;
				RaisePropertyChanged(() => NeedReboot);
			}
		}

		private TimeSpan _timeOfNotify;
		public TimeSpan TimeOfNotify
		{
			get
			{
				return _timeOfNotify;
			}

			set
			{
				_timeOfNotify = value;
				TimeChanged();
				RaisePropertyChanged(() => TimeOfNotify);
			}
		}

		#endregion

		public List<Language> Languages { get; } = new List<Language>
		{
			new Language{Name = "Русский",Id = "ru"},
			new Language{Name = "Беларускі",Id = "be"},
			new Language{Name = "English",Id = "en"}
		};

		private Language _selectedLanguage;
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
				SaveChanges();
			}
		}

		private bool _isStationsDownloading;
		public bool IsStationsDownloading
		{
			get
			{
				return _isStationsDownloading;
			}

			set
			{
				_isStationsDownloading = value;
				RaisePropertyChanged(() => IsStationsDownloading);
			}
		}

		private List<Country> _countries;
		public List<Country> Countries
		{
			get
			{
				return _countries;
			}

			set
			{
				_countries = value;
				RaisePropertyChanged(() => Countries);
			}
		}

		private Country _selectedCountry;

		public Country SelectedCountry
		{
			get
			{
				return _selectedCountry;
			}

			set
			{
				_selectedCountry = value;
				RaisePropertyChanged(() => SelectedCountry);
			}
		}

		private bool _isAllCountriesDownloaded;

		public bool IsAllCountriesDownloaded
		{
			get
			{
				return _isAllCountriesDownloaded;
			}

			set
			{
				_isAllCountriesDownloaded = value;
				RaisePropertyChanged(() => IsAllCountriesDownloaded);
			}
		}

		#endregion

		#region actions

		/// <summary>
		/// Invoked when this page is about to be displayed in a Frame.
		/// </summary>
		public void Init()
		{
			RestoreUiBinding();
			if (_appSettings.Language == null)
				_appSettings.Language = new Language { Id = "ru" };
			SelectedLanguage = Languages.First(x => x.Id == _appSettings.Language.Id);
			Countries = _appSettings.AutoCompletion.Skip(Common.NumberOfBelarussianStopPoints).Any() ?
				new List<Country>(_appSettings.Countries.Except(_appSettings.AutoCompletion.Skip(Common.NumberOfBelarussianStopPoints).GroupBy(x => x.LabelTail).First().Select(x => new Country { Name = x.LabelTail }))) :
				_appSettings.Countries;
			SelectedCountry = Countries.FirstOrDefault();
			_timeOfNotify = _appSettings.Reminder;
			CheckIsAllCountriesDownloaded();
		}

		private void SaveChanges()
		{
			if (SelectedLanguage.Id != _appSettings.Language.Id)
			{
				_analytics.SentEvent(Analytics.LanguageChanged, SelectedLanguage.Name);
				_serializable.Serialize(SelectedLanguage, Restoring.CurrentLanguage);
				NeedReboot = _localizationService.GetString("NeedReboot");
			}
			else
			{
				NeedReboot = Empty;
				_serializable.Serialize(_appSettings.Language, Restoring.CurrentLanguage);
			}
		}

		private void ResetSetting()
		{
			_serializable.Delete(Common.IsFirstRun);
			NeedReboot = _localizationService.GetString("NeedReboot");
		}

		private async void DownloadCountryStopPoint()
		{
			if (SelectedCountry == null)
				return;
			IsStationsDownloading = true;

			var countryStopPoints = await _local.GetLanguageData<List<CountryStopPointItem>>($"{Defines.Uri.CountriesFolder}{SelectedCountry.Name}.json");
			if (countryStopPoints != null && countryStopPoints.Any())
			{
				foreach (var countryStopPoint in countryStopPoints)
					_appSettings.AutoCompletion.Add(countryStopPoint);
				_serializable.Serialize(_appSettings, Restoring.AppSettings);
				Countries.Remove(SelectedCountry);
				await _userInteraction.AlertAsync(
					$"{SelectedCountry.Name}{' '}{_localizationService.GetString("CountrySuccessfullyAdded")}");

				CheckIsAllCountriesDownloaded();

				SelectedCountry = Countries.FirstOrDefault();
			}

			else
			{
				await _userInteraction.AlertAsync(_localizationService.GetString("CountryCanNotDownloaded"));
				_analytics.SentEvent("exception", "Contries", SelectedCountry.Name);
			}

			IsStationsDownloading = false;
		}

		private void CheckIsAllCountriesDownloaded()
		{
			if (!Countries.Any())
				IsAllCountriesDownloaded = true;
		}

		private void TimeChanged()
		{
			_appSettings.Reminder = TimeOfNotify;
			_serializable.Serialize(_appSettings, Restoring.AppSettings);
		}

		private void RestoreUiBinding()
		{
			Header = _localizationService.GetString("Settings");
			TimeNotification = _localizationService.GetString("TimeNotification");
			SelectLanguage = _localizationService.GetString("SelectLanguage");
			ResetSettings = _localizationService.GetString("ResetSettings");
			SelectCountries = _localizationService.GetString("SelectCountries");
			DownloadSelectCountry = _localizationService.GetString("DownloadSelectCountry");
		}

		#endregion
	}
}