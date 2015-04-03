using Cirrious.MvvmCross.ViewModels;
using System.Collections.Generic;
using Trains.Model.Entities;

namespace Trains.Core.ViewModels
{
    public class HelpViewModel : MvxViewModel
    {
        #region properties

        /// <summary>
        /// Used to dispalying informations about belarussian railway icons.
        /// </summary>
        private static IEnumerable<HelpInformationItem> _menu;
        public IEnumerable<HelpInformationItem> Menu
        {
            get { return _menu; }
            set
            {
                _menu = value;
                RaisePropertyChanged(() => Menu);
            }
        }

        #endregion

        #region action

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        public async void Init()
        {
            //Menu = await MenuData.GetItemsAsync();
        }

        #endregion
    }
}