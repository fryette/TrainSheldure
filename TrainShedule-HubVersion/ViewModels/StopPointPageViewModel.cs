using System.Collections.Generic;
using Caliburn.Micro;
using TrainSearch.Entities;

namespace Trains.App.ViewModels
{
    /// <summary>
    /// Keep information about stop point.
    /// </summary>
    public class StopPointPageViewModel : Screen
    {
        #region readonlyProperties
        /// <summary>
        /// Used to navigate between pages.
        /// </summary>
        private readonly INavigationService _navigationService;

        /// <summary>
        /// Used to display stop points and time of arrival and time of departure.
        /// </summary> 
        public IEnumerable<TrainStop> Parameter { get; set; }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// Set the default parameter of some properties.
        /// </summary>
        protected override void OnActivate() { }
        #endregion

        #region actions
        public StopPointPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        /// <summary>
        /// Go to saved by user routes page.
        /// </summary>
        private void GoToHelpPage()
        {
            _navigationService.NavigateToViewModel<HelpPageViewModel>();
        }
        #endregion
    }

}
