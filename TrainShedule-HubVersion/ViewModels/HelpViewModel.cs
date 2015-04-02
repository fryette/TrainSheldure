using System.Collections.Generic;
using Caliburn.Micro;
using Trains.Infrastructure.Infrastructure;
using Trains.Model.Entities;

namespace Trains.App.ViewModels
{
    /// <summary>
    /// Used to dispalying informations about belarussian railway icons.
    /// </summary>
    public class HelpViewModel : Screen
    {

        #region properties
        
        /// <summary>
        /// Used to dispalying informations about belarussian railway icons.
        /// </summary>
        private static IEnumerable<MenuDataItem> _menu;
        public IEnumerable<MenuDataItem> Menu
        {
            get { return _menu; }
            set
            {
                _menu = value;
                NotifyOfPropertyChange(() => Menu);
            }
        }

        #endregion

        #region action

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        protected override async void OnActivate()
        {
            Menu = await MenuData.GetItemsAsync();
        }

        #endregion
    }
}
