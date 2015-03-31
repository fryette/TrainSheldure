using System;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using Trains.Model.Entities;
using Trains.Services.Interfaces;
using Trains.Entities;

namespace Trains.App.ViewModels
{
    /// <summary>
    /// Shows train schedule.
    /// </summary> 
    public class ScheduleViewModel : Screen
    {
        #region properties
        /// <summary>
        /// Сontains information on all trains on the route selected by the user.
        /// </summary> 
        public IEnumerable<Train> Parameter { get; set; }

        public string Request { get; set; }

        /// <summary>
        /// Used to navigate between pages.
        /// </summary>
        private readonly INavigationService _navigationService;

        /// <summary>
        /// Used to serialization/deserialization objects.
        /// </summary>
        private readonly ISerializableService _serializable;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="navigationService">Used to navigate between pages.</param>
        /// <param name="serializable">Used to serialization/deserialization objects.</param>
        public ScheduleViewModel(INavigationService navigationService, ISerializableService serializable)
        {
            _navigationService = navigationService;
            _serializable = serializable;
        }
        #endregion

        #region action
        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// Set the default parameter of some properties.
        /// </summary>
        protected override void OnActivate()
        {
            Request = SavedItems.UpdatedLastRequest.From + " - " + SavedItems.UpdatedLastRequest.To;
            _serializable.SerializeObjectToXml(Parameter.ToList(), "LastTrainList");
        }

        /// <summary>
        /// Invoked when the user selects his train of interest.
        /// Go to the information page which show additional information about this train.
        /// </summary>
        /// <param name="train">Data that describes user-selected train(prices,seats,stop points and other)</param>
        private void ClickItem(Train train)
        {
            _navigationService.NavigateToViewModel<InformationViewModel>(train);
        }

        private void ClickItem1(Object train)
        {
            //_navigationService.NavigateToViewModel<InformationViewModel>(train);
        }

        /// <summary>
        /// Go to saved by user routes page.
        /// </summary>
        private void GoToHelpPage()
        {
            _navigationService.NavigateToViewModel<HelpViewModel>();
        }
        #endregion
    }
}
