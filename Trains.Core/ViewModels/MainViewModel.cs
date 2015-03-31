using System.Collections.Generic;
using Cirrious.MvvmCross.ViewModels;
using Trains.Entities;
using Trains.Model.Entities;

namespace Trains.Core.ViewModels
{
    public class MainViewModel : MvxViewModel
    {
        #region readonlyProperties
        /// <summary>
        /// Used to get trains from the last request.
        /// </summary>
        //private readonly ILastRequestTrainService _lastRequestTrain;

        //private readonly IStartService _start;

        ///// <summary>
        ///// Used to serialization/deserialization objects.
        ///// </summary>
        //private readonly ISerializableService _serializable;

        ///// <summary>
        ///// Used to search train schedule.
        ///// </summary>
        //private readonly ISearchService _search;

        #endregion

        #region ctor

        public MainViewModel()
        {

        }

        #endregion

        #region commands

        #endregion

        #region properties

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

        /// <summary>
        /// Used for process control.
        /// </summary>
        private bool _isBarDownloaded;
        public bool IsBarDownloaded
        {
            get { return _isBarDownloaded; }
            set
            {
                _isBarDownloaded = value;
                RaisePropertyChanged(() => IsBarDownloaded);
            }
        }

        /// <summary>
        /// Used for process download data control.
        /// </summary>
        private bool _isDownloadRun;
        public bool IsDownloadRun
        {
            get { return _isDownloadRun; }
            set
            {
                _isDownloadRun = value;
                RaisePropertyChanged(() => IsDownloadRun);
            }
        }

        /// <summary>
        /// Keeps trains from the last request.
        /// </summary>
        private static List<Train> _trains;
        public List<Train> Trains
        {
            get { return _trains; }
            set
            {
                _trains = value;
                RaisePropertyChanged(() => Trains);
            }
        }

        /// <summary>
        /// Object are stored custom routes.
        /// </summary>
        private IEnumerable<LastRequest> _favoriteRequests;
        public IEnumerable<LastRequest> FavoriteRequests
        {
            get { return _favoriteRequests; }
            set
            {
                _favoriteRequests = value;
                RaisePropertyChanged(() => FavoriteRequests);
            }
        }

        /// <summary>
        /// Last route
        /// </summary>
        private string _lastRoute;

        public string LastRoute
        {
            get { return _lastRoute; }
            set
            {
                _lastRoute = value;
                RaisePropertyChanged(() => LastRoute);
            }
        }

        #endregion

        #region actions

        #endregion

    }
}
