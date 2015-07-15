using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Chance.MvvmCross.Plugins.UserInteraction;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using Trains.Core.Interfaces;
using Trains.Core.Resources;
using Trains.Core.Services.Interfaces;
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


		#endregion

		#region command

		public IMvxCommand ResetSettingsCommand { get; private set; }

		#endregion

		#region ctor

		public SettingsViewModel(ISerializableService serializable, IAppSettings appSettings, IAnalytics analytics, ILocalDataService local)
		{
			ResetSettingsCommand = new MvxCommand(ResetSetting);

			_analytics = analytics;
			_serializable = serializable;
			_appSettings = appSettings;
			_local = local;
		}

		#endregion

		#region properties

		#region UIproperties

		public string Header { get; set; }

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
		public string SelectLanguage { get; set; }
		public string ResetSettings { get; set; }

		#endregion

		public List<Language> Languages
		{ get; }
		= new List<Language>
		{
			new Language{Name = "Русский",Id = "ru"},
			new Language{Name = "Беларускі",Id = "be"},
			new Language{Name = "English",Id = "en"}
		};

		/// <summary>
		/// Used to set code behind variant of search.
		/// </summary> 
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

			Countries = _appSettings.Countries.Where(x=>
			{
				return !_appSettings.AutoCompletion.Any(item => item.Label.Contains(x.Name));
			}).ToList();

			SelectedLanguage = Languages.First(x => x.Id == _appSettings.Language.Id);

			//Countries = _appSettings.Countries;
		}

		private void SaveChanges()
		{
			if (SelectedLanguage.Id != _appSettings.Language.Id)
			{
				_analytics.SentEvent(Constants.LanguageChanged, SelectedLanguage.Name);
				_serializable.Serialize(SelectedLanguage, Constants.CurrentLanguage);
				NeedReboot = ResourceLoader.Instance.Resource["NeedReboot"];
			}
			else
			{
				NeedReboot = Empty;
				_serializable.Serialize(_appSettings.Language, Constants.CurrentLanguage);
			}
		}

		private void ResetSetting()
		{
			_serializable.Delete(Constants.IsFirstRun);
			NeedReboot = ResourceLoader.Instance.Resource["NeedReboot"];
		}

		private async void DownloadCountryStopPoint()
		{
			if (_appSettings.AutoCompletion.Any(x => x.Label.Contains(SelectLanguage)))
				await Mvx.Resolve<IUserInteraction>().AlertAsync("Уже загружена");

			IsStationsDownloading = true;
			var countryStopPoints = await _local.GetLanguageData<List<CountryStopPointItem>>(Constants.CountriesFolder + SelectLanguage + ".json");

			foreach (var countryStopPoint in countryStopPoints)
			{
				_appSettings.AutoCompletion.Add(countryStopPoint);
			}
			
			IsStationsDownloading = false;
		}

		private void RestoreUiBinding()
		{
			Header = ResourceLoader.Instance.Resource["Settings"];
			SelectLanguage = ResourceLoader.Instance.Resource["SelectLanguage"];
			ResetSettings = ResourceLoader.Instance.Resource["ResetSettings"];
		}

		#endregion
	}
}