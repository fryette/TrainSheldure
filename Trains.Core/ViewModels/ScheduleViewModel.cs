using Cirrious.MvvmCross.ViewModels;
using Newtonsoft.Json;
using System.Collections.Generic;
using Trains.Entities;
using Trains.Services.Interfaces;
using System.Linq;
using Trains.Infrastructure.Interfaces;

namespace Trains.Core.ViewModels
{
    public class ScheduleViewModel : MvxViewModel
    {
        #region readonlyProperty

        /// <summary>
        /// Used to serialization/deserialization objects.
        /// </summary>
        private readonly ISerializableService _serializable;

        private readonly IAppSettings _appSettings;

        #endregion

        #region command

        public IMvxCommand GoToHelpPageCommand { get; private set; }
        public IMvxCommand SelectTrainCommand { get; private set; }

        #endregion

        #region ctor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="navigationService">Used to navigate between pages.</param>
        /// <param name="serializable">Used to serialization/deserialization objects.</param>
        public ScheduleViewModel(ISerializableService serializable, IAppSettings appSettings)
        {
            _appSettings = appSettings;
            _serializable = serializable;

            GoToHelpPageCommand = new MvxCommand(GoToHelpPage);
            SelectTrainCommand = new MvxCommand(() => ClickItem(SelectedTrain));
        }

        #endregion

        #region properties

        /// <summary>
        /// �ontains information on all trains on the route selected by the user.
        /// </summary> 
        public IEnumerable<Train> Trains { get; set; }

        public string Request { get; set; }

        private Train _selectedTrain;
        public Train SelectedTrain
        {
            get
            {
                return _selectedTrain;
            }
            set
            {
                _selectedTrain = value;
                RaisePropertyChanged(() => SelectedTrain);
            }
        }

        #endregion


        #region action
        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// Set the default parameter of some properties.
        /// </summary>
        public void Init(string param)
        {
            Trains = JsonConvert.DeserializeObject<List<Train>>(param);
            Request = _appSettings.UpdatedLastRequest.From + " - " + _appSettings.UpdatedLastRequest.To;
        }

        /// <summary>
        /// Invoked when the user selects his train of interest.
        /// Go to the information page which show additional information about this train.
        /// </summary>
        /// <param name="train">Data that describes user-selected train(prices,seats,stop points and other)</param>
        private void ClickItem(Train train)
        {
            ShowViewModel<InformationViewModel>(new { param = JsonConvert.SerializeObject(train) });
        }

        /// <summary>
        /// Go to saved by user routes page.
        /// </summary>
        private void GoToHelpPage()
        {
            ShowViewModel<HelpViewModel>();
        }

        #endregion
    }
}