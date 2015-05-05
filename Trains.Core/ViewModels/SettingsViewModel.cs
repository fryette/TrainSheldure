using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chance.MvvmCross.Plugins.UserInteraction;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using Trains.Core.Interfaces;
using Trains.Core.Resources;
using Trains.Core.Services.Interfaces;
using Trains.Model.Entities;
using System;

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

            _local = local;
            _analytics = analytics;
            _serializable = serializable;
            _appSettings = appSettings;
        }

        #endregion

        #region properties

        #region UIproperties

        public string Header { get; set; }
        private string _needReboot { get; set; }
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

        /// <summary>
        /// Languages
        /// </summary>
        private readonly List<Language> _languagesList = new List<Language>
        {
            new Language{Name = "Русский",Id = "ru"},
            new Language{Name = "Беларускі",Id = "be"},
            new Language{Name = "English",Id = "en"}
        };
        public List<Language> Languages
        {
            get { return _languagesList; }
        }

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

        #endregion

        #region actions

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        public void Init()
        {
            RestoreUI();
            if (_appSettings.Language == null)
                _appSettings.Language = new Language { Id = "ru" };
            SelectedLanguage = _languagesList.First(x => x.Id == _appSettings.Language.Id);
        }

        private void SaveChanges()
        {
            if (SelectedLanguage.Id != _appSettings.Language.Id)
            {
                _analytics.SentEvent(Constants.LanguageChanged, SelectedLanguage.Name);
                _serializable.Serialize<Language>(SelectedLanguage, Constants.CurrentLanguage);
                NeedReboot = ResourceLoader.Instance.Resource["NeedReboot"];
            }
            else
            {
                NeedReboot = String.Empty;
                _serializable.Serialize<Language>(_appSettings.Language, Constants.CurrentLanguage);
            }
        }

        private void ResetSetting()
        {
            _serializable.Delete(Constants.IsFirstRun);
            NeedReboot = ResourceLoader.Instance.Resource["NeedReboot"];
        }

        private void RestoreUI()
        {
            Header = ResourceLoader.Instance.Resource["Settings"];
            SelectLanguage = ResourceLoader.Instance.Resource["SelectLanguage"];
            ResetSettings = ResourceLoader.Instance.Resource["ResetSettings"];
        }

        #endregion
    }
}