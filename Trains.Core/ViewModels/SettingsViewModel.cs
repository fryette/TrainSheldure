﻿using System;
using System.Collections.Generic;
using System.Linq;
using Chance.MvvmCross.Plugins.UserInteraction;
using Cirrious.MvvmCross.ViewModels;
using Trains.Infrastructure;
using Trains.Infrastructure.Interfaces;
using Trains.Infrastructure.Interfaces.Platform;
using Trains.Infrastructure.Interfaces.Services;
using Trains.Model.Entities;
using static System.String;

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
		public void Init()
		{
			Countries = _appSettings.AutoCompletion.Skip(Defines.Common.NumberOfBelarussianStopPoints).Any() ?
				new List<Country>(_appSettings.Countries.Except(_appSettings.AutoCompletion.Skip(Defines.Common.NumberOfBelarussianStopPoints).GroupBy(x => x.LabelTail).First().Select(x => new Country { Name = x.LabelTail }))) :
				_appSettings.Countries;
			SelectedCountry = Countries.FirstOrDefault();
			_timeOfNotify = _appSettings.Reminder;
			CheckIsAllCountriesDownloaded();
		}

		private void ResetSetting()
		{
			_serializable.Delete(Defines.Restoring.AppLanguage);
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
				_serializable.Serialize(_appSettings, Defines.Restoring.AppSettings);
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
			_serializable.Serialize(_appSettings, Defines.Restoring.AppSettings);
		}
		#endregion
	}
}