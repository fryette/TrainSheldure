﻿using System.Collections.Generic;
using System.Linq;
using Windows.ApplicationModel.Resources;
using Windows.Globalization;
using Caliburn.Micro;
using Trains.Model.Entities;
using Trains.Services.Interfaces;
using Trains.Services.Tools;
using Language = Trains.Model.Entities.Language;

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
            new Language{Name = "Белорусский",Id = "be"}
        };
        public List<Language> Languages
        {
            get { return _languagesList; }
        }
        /// <summary>
        /// Used to set code behind variant of search.
        /// </summary> 
        private Language _selectedLanguages;
        public Language SelectedLanguages
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
            SelectedLanguages = _languagesList.First(x => x.Id == SavedItems.ResourceLoader.GetString("Culture"));
        }

        private void SaveChanges() 
        {
            _serialize.SerializeObjectToXml(SelectedLanguages, "currentLanguage");
            _serialize.DeleteFile("lastRequests");
            ApplicationLanguages.PrimaryLanguageOverride = SelectedLanguages.Id;
            ToolHelper.ShowMessageBox(SavedItems.ResourceLoader.GetString("LanguageChanged"));
        }
        #endregion
    }
}
