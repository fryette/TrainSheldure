using Chance.MvvmCross.Plugins.UserInteraction;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using System.Collections.Generic;
using Trains.Core.Interfaces;
using Trains.Model.Entities;
using Trains.Resources;
using System.Linq;

namespace Trains.Core.ViewModels
{
    public class SettingsViewModel : MvxViewModel
    {
        #region readonlyProperties

        private readonly ISerializableService _serialize;
        private readonly IAppSettings _appSettings;
        private readonly IAnalytics _analytics;
        private readonly IManageLangService _manageLang;

        #endregion

        #region command

        public IMvxCommand SaveChangesCommand { get; private set; }

        #endregion

        #region ctor

        public SettingsViewModel(ISerializableService serializable, IAppSettings appSettings, IManageLangService manageLang, IAnalytics analytics)
        {
            SaveChangesCommand = new MvxCommand(SaveChanges);

            _analytics = analytics;
            _manageLang = manageLang;
            _serialize = serializable;
            _appSettings = appSettings;
        }

        #endregion

        #region properties

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
            SelectedLanguage = _languagesList.First(x => x.Id == _appSettings.Language.Id);
        }

        private async void SaveChanges()
        {
            var result = await Mvx.Resolve<IUserInteraction>().ConfirmAsync(ResourceLoader.Instance.Resource["DeleteDataNotification"], ResourceLoader.Instance.Resource["Warning"]);
            if (!result) return;
            _analytics.SentEvent("LanguageChanged", SelectedLanguage.Name);
            _manageLang.ChangeAppLanguage(SelectedLanguage.Id);
            await _serialize.SerializeObjectToXml(SelectedLanguage, Constants.CurrentLanguage);
            await _serialize.DeleteFile(Constants.FavoriteRequests);
            await _serialize.DeleteFile(Constants.LastTrainList);
            await _serialize.DeleteFile(Constants.UpdateLastRequest);

            await Mvx.Resolve<IUserInteraction>().AlertAsync(ResourceLoader.Instance.Resource["LanguageChanged"]);
        }

        #endregion
    }
}