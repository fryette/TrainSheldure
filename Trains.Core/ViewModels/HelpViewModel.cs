using Cirrious.MvvmCross.ViewModels;
using System.Collections.Generic;
using Trains.Model.Entities;
using Trains.Services.Interfaces;

namespace Trains.Core.ViewModels
{
    public class HelpViewModel : MvxViewModel
    {
        #region readonlyProperties

        private readonly IAppSettings _appSettings;
        
        #endregion

        #region ctor

        public HelpViewModel(IAppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        #endregion

        #region properties

        /// <summary>
        /// Used to dispalying informations about belarussian railway icons.
        /// </summary>
        private static IEnumerable<HelpInformationItem> _helpInformation;
        public IEnumerable<HelpInformationItem> HelpInformation
        {
            get { return _helpInformation; }
            set
            {
                _helpInformation = value;
                RaisePropertyChanged(() => HelpInformation);
            }
        }

        #endregion

        #region action

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        public void Init()
        {
            HelpInformation = _appSettings.HelpInformation;
        }

        #endregion
    }
}