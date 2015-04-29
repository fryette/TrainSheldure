using Chance.MvvmCross.Plugins.UserInteraction;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using System.Collections.Generic;
using Trains.Core.Interfaces;
using Trains.Model.Entities;
using Trains.Core;
using System.Linq;
using System.Threading.Tasks;
using Cirrious.MvvmCross.Plugins.File;

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

        public SettingsViewModel(ISerializableService serializable, IAppSettings appSettings,IAnalytics analytics, ILocalDataService local)
        {
            SaveChangesCommand = new MvxCommand(SaveChanges);

            _local = local;
            _analytics = analytics;
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
            if (_appSettings.Language == null)
                _appSettings.Language = new Language { Id = "ru" };
            SelectedLanguage = _languagesList.First(x => x.Id == _appSettings.Language.Id);
        }

        private async void SaveChanges()
        {
            //var result = await Mvx.Resolve<IUserInteraction>().ConfirmAsync(ResourceLoader.Instance.Resource["DeleteDataNotification"], ResourceLoader.Instance.Resource["Warning"]);
            //if (!result) return;
            _analytics.SentEvent(Constants.LanguageChanged, SelectedLanguage.Name);
            _serialize.Serialize(SelectedLanguage, Constants.CurrentLanguage);
            _serialize.Delete(Constants.FavoriteRequests);
            _serialize.Delete(Constants.LastTrainList);
            _serialize.Delete(Constants.UpdateLastRequest);
            await DowloadResources();
            _serialize.Serialize(_appSettings, Constants.AppSettings);
            _serialize.Serialize(ResourceLoader.Instance.Resource, Constants.ResourceLoader);
            await Mvx.Resolve<IUserInteraction>().AlertAsync(ResourceLoader.Instance.Resource["LanguageChanged"]);
        }

        private async Task DowloadResources()
        {
            var initializerResourceLoader = ResourceLoader.Instance;
            _appSettings.AutoCompletion = await _local.GetData<List<CountryStopPointItem>>(Constants.StopPointsJson);
            _appSettings.HelpInformation = await _local.GetData<List<HelpInformationItem>>(Constants.HelpInformationJson);
            _appSettings.CarriageModel = await _local.GetData<List<CarriageModel>>(Constants.CarriageModelJson);
            _appSettings.About = await _local.GetData<List<About>>(Constants.AboutJson);
        }

        #endregion
    }
}