using Cirrious.MvvmCross.ViewModels;
using Trains.Entities;
using Trains.Services.Interfaces;
using Newtonsoft.Json;

namespace Trains.Core.ViewModels
{
    public class InformationViewModel : MvxViewModel
    {
        #region readonlyProperties

        /// <summary>
        /// Used to grab train stops.
        /// </summary>
        private readonly ITrainStopService _trainStop;

        #endregion

        #region command

        public IMvxCommand SearchStopPointsCommand { get; private set; }

        #endregion

        #region ctor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="trainStop">Used to grab train stops.</param>
        public InformationViewModel(ITrainStopService trainStop)
        {
            _trainStop = trainStop;
            SearchStopPointsCommand = new MvxCommand(SearchStopPoint);
        }

        #endregion

        #region properties

        /// <summary>
        /// User-selected train.
        /// </summary>
        public Train Parameter { get; set; }

        /// <summary>
        /// User-selected train.
        /// Used to save train if user whant to search TrainStops
        /// </summary>        
        private static Train SavedLastTrainInformations { get; set; }

        /// <summary>
        /// Used to dispalying informations about the seats and their prices.
        /// </summary>
        private AdditionalInformation[] _additionalInformation;

        public AdditionalInformation[] AdditionalInformation
        {
            get { return _additionalInformation; }
            set
            {
                _additionalInformation = value;
                RaisePropertyChanged(() => AdditionalInformation);
            }
        }

        /// <summary>
        /// Used for process control.
        /// </summary>
        private bool _isTaskRun;

        public bool IsTaskRun
        {
            get { return _isTaskRun; }
            set
            {
                _isTaskRun = value;
                RaisePropertyChanged(() => IsTaskRun);
            }
        }

        #endregion

        #region action

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// Set the additional informations about user-selected train.
        /// </summary>
        public void Init()
        {
            if (Parameter == null)
                Parameter = SavedLastTrainInformations;
            else
                AdditionalInformation = Parameter.AdditionalInformation;
        }

        /// <summary>
        /// Search additional information about user-selected train.
        /// </summary>
        private async void SearchStopPoint()
        {
            if (IsTaskRun) return;
            IsTaskRun = true;
            SavedLastTrainInformations = Parameter;
            var stopPointList = await _trainStop.GetTrainStop(Parameter.Link);
            IsTaskRun = false;
            ShowViewModel<StopPointViewModel>(new { param = JsonConvert.SerializeObject(stopPointList) });
        }

        #endregion
    }
}