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

        private readonly ISerializableService _serialize;
        private readonly IAppSettings _appSettings;
        private readonly IAnalytics _analytics;
        private readonly ILocalDataService _local;

        #endregion

        #region command

        public IMvxCommand SaveChangesCommand { get; private set; }

        #endregion

        #region ctor

        public SettingsViewModel(ISerializableService serializable, IAppSettings appSettings, IAnalytics analytics, ILocalDataService local)
        {
            SaveChangesCommand = new MvxCommand(SaveChanges);

            _local = local;
            _analytics = analytics;
            _serialize = serializable;
            _appSettings = appSettings;
        }

        #endregion

        #region properties

        private bool _saveRun;
        public bool SaveRun
        {
            get
            {
                return _saveRun;
            }
            set
            {
                _saveRun = value;
                RaisePropertyChanged(() => SaveRun);
            }
        }

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
            }
        }

        #endregion

        #region actions

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        public void Init()
        {
            if (_appSettings.Language == null)
                _appSettings.Language = new Language { Id = "ru" };
            SelectedLanguage = _languagesList.First(x => x.Id == _appSettings.Language.Id);
        }

        private async void SaveChanges()
        {
            if (SaveRun) return;
            SaveRun = true;
            _analytics.SentEvent(Constants.LanguageChanged, SelectedLanguage.Name);
            _appSettings.Language = SelectedLanguage;
            bool isException = false;
            try
            {
                await DowloadResources();
                DeleteSaveSettings();
            }
            catch (Exception e)
            {
                isException = true;
                _analytics.SentException(e.Message, true);
            }
            if (isException)
                await Mvx.Resolve<IUserInteraction>().AlertAsync("Произошла проблема с загрузкой ресурсов,проверьте доступ к интернету и повторите");
            else
                await Mvx.Resolve<IUserInteraction>().AlertAsync(ResourceLoader.Instance.Resource["LanguageChanged"]);
            SaveRun = false;
        }

        private async Task DowloadResources()
        {
            _appSettings.AutoCompletion = await _local.GetLanguageData<List<CountryStopPointItem>>(Constants.StopPointsJson);
            _appSettings.HelpInformation = await _local.GetLanguageData<List<HelpInformationItem>>(Constants.HelpInformationJson);
            _appSettings.CarriageModel = await _local.GetLanguageData<List<CarriageModel>>(Constants.CarriageModelJson);
            _appSettings.About = await _local.GetLanguageData<List<About>>(Constants.AboutJson);
            _serialize.Serialize(await _local.GetLanguageData<Dictionary<string, string>>(Constants.ResourceJson), Constants.ResourceLoader);
            _serialize.Serialize(await _local.GetOtherData<Patterns>(Constants.PatternsJson), Constants.Patterns);
            _appSettings.SocialUri = await _local.GetOtherData<SocialUri>(Constants.SocialJson);
            _serialize.Serialize(_appSettings, Constants.AppSettings);
            _serialize.Serialize(SelectedLanguage, Constants.CurrentLanguage);
            _appSettings.AutoCompletion = null;
        }

        private void DeleteSaveSettings()
        {
            _serialize.Delete(Constants.FavoriteRequests);
            _serialize.Delete(Constants.LastTrainList);
            _serialize.Delete(Constants.UpdateLastRequest);
        }

        #endregion
    }
}