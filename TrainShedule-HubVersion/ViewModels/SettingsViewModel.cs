using System.Collections.Generic;
using System.Linq;
using Windows.ApplicationModel.Resources;
using Caliburn.Micro;
using Trains.Model.Entities;
using Trains.Services.Interfaces;

namespace Trains.App.ViewModels
{
    public class SettingsViewModel : Screen
    {
        #region properties

        private readonly ISerializableService _serialize;

        /// <summary>
        /// Languages
        /// </summary>
        private readonly List<Language> _languagesList = new List<Language>
        {
            new Language{Name = "Русский",Id = "ru"},
            new Language{Name = "Беларусский",Id = "be"}
        };
        public IEnumerable<string> Languages
        {
            get { return _languagesList.Select(x => x.Name); }
        }
        /// <summary>
        /// Used to set code behind variant of search.
        /// </summary> 
        private string _selectedLanguages;
        public string SelectedLanguages
        {
            get
            {
                return _selectedLanguages;
            }

            set
            {
                _selectedLanguages = value;
                NotifyOfPropertyChange(() => SelectedLanguages);
            }
        }
        #endregion

        #region ctor

        public SettingsViewModel(ISerializableService serializable)
        {
            _serialize = serializable;
        }
        #endregion

        #region actions
        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        protected override void OnActivate()
        {
            SelectedLanguages = _languagesList.First(x => x.Id == SavedItems.ResourceLoader.GetString("Culture")).Name;
        }

        private void SaveChanges()
        {
            var lang = _languagesList.First(x => x.Name == SelectedLanguages);
            _serialize.SerializeObjectToXml(lang, "currentLanguage");
            Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = lang.Id;
            SavedItems.ResourceLoader = ResourceLoader.GetForViewIndependentUse("Resources");
        }
        #endregion
    }
}
