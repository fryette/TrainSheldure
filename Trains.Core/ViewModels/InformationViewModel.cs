using System.Collections.Generic;
using System.Threading.Tasks;
using Cirrious.MvvmCross.ViewModels;
using Newtonsoft.Json;
using Trains.Core.Services.Interfaces;
using Trains.Entities;
using Trains.Model.Entities;

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

        #region ctor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="trainStop">Used to grab train stops.</param>
        public InformationViewModel(ITrainStopService trainStop)
        {
            _trainStop = trainStop;
        }

        #endregion

        #region properties

        #region UIproperties

        public string DownloadStopPoints { get; set; }
        
        #endregion

        public IEnumerable<TrainStop> _stopPointList;
        public IEnumerable<TrainStop> StopPointList
        {
            get { return _stopPointList; }
            set
            {
                _stopPointList = value;
                RaisePropertyChanged(() => StopPointList);
            }
        }

        /// <summary>
        /// User-selected train.
        /// </summary>
        public Train Train { get; set; }

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
        public void Init(string param)
        {
            RestoreUIBindings();
            IsTaskRun = true;
            Train = JsonConvert.DeserializeObject<Train>(param);
            SearchStopPoint();
        }

        /// <summary>
        /// Search additional information about user-selected train.
        /// </summary>
        private async Task SearchStopPoint()
        {
            StopPointList = (await _trainStop.GetTrainStop(Train.Link));
            IsTaskRun = false;
        }

        private void RestoreUIBindings()
        {
            DownloadStopPoints = ResourceLoader.Instance.Resource["DownloadStopPoints"];
        }
        #endregion
    }
}