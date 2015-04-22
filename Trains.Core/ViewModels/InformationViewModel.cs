using Cirrious.MvvmCross.ViewModels;
using Newtonsoft.Json;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Trains.Core.Interfaces;
using Trains.Entities;
using Trains.Model.Entities;
using Trains.Services.Interfaces;

namespace Trains.Core.ViewModels
{
    public class InformationViewModel : MvxViewModel
    {
        #region readonlyProperties

        /// <summary>
        /// Used to grab train stops.
        /// </summary>
        private readonly ITrainStopService _trainStop;
        private readonly IFavoriteManageService _manageFavoriteRequest;
        private readonly IAppSettings _appSettings;

        #endregion

        #region command

        public IMvxCommand AddToFavoriteCommand { get; private set; }
        public IMvxCommand DeleteInFavoriteCommand { get; private set; }
        public IMvxCommand SearchStopPointsCommand { get; private set; }

        #endregion

        #region ctor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="trainStop">Used to grab train stops.</param>
        public InformationViewModel(ITrainStopService trainStop, IFavoriteManageService manageFavoriteRequest, IAppSettings appSettings)
        {
            _trainStop = trainStop;
            _manageFavoriteRequest = manageFavoriteRequest;
            _appSettings = appSettings;

            AddToFavoriteCommand = new MvxCommand(AddToFavorite);
            DeleteInFavoriteCommand = new MvxCommand(DeleteInFavorite);
        }

        #endregion

        #region properties

        /// <summary>
        /// Used to display favorite icon.
        /// </summary> 
        private bool _isVisibleFavoriteIcon;
        public bool IsVisibleFavoriteIcon
        {
            get
            {
                return _isVisibleFavoriteIcon;
            }

            set
            {
                _isVisibleFavoriteIcon = value;
                RaisePropertyChanged(() => IsVisibleFavoriteIcon);
            }
        }

        /// <summary>
        /// Used to display unfavorite icon.
        /// </summary> 
        private bool _isVisibleUnFavoriteIcon;
        public bool IsVisibleUnFavoriteIcon
        {
            get
            {
                return _isVisibleUnFavoriteIcon;
            }

            set
            {
                _isVisibleUnFavoriteIcon = value;
                RaisePropertyChanged(() => IsVisibleUnFavoriteIcon);
            }
        }

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
            IsTaskRun = true;
            Train = JsonConvert.DeserializeObject<Train>(param);
            SetManageFavoriteButton();
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

        private void SetManageFavoriteButton()
        {
            if (_appSettings.FavoriteRequests == null) SetVisibilityToFavoriteIcons(true, false);
            else if (_appSettings.FavoriteRequests.Any(x => x.From == _appSettings.UpdatedLastRequest.From && x.To == _appSettings.UpdatedLastRequest.To))
                SetVisibilityToFavoriteIcons(false, true);
            else SetVisibilityToFavoriteIcons(true, false);
        }

        /// <summary>
        /// Saves the entered route to favorite.
        /// </summary>
        private async void AddToFavorite()
        {
            if (await Task.Run(() => _manageFavoriteRequest.AddToFavorite(_appSettings.UpdatedLastRequest.From, _appSettings.UpdatedLastRequest.To)))
                SetVisibilityToFavoriteIcons(false, true);
        }

        /// <summary>
        /// Deletes the entered route visas favorite.
        /// </summary>
        private async void DeleteInFavorite()
        {
            if (await Task.Run(() => _manageFavoriteRequest.DeleteRoute(_appSettings.UpdatedLastRequest.From, _appSettings.UpdatedLastRequest.To)))
                SetVisibilityToFavoriteIcons(true, false);
        }

        /// <summary>
        /// Change visibility of favorite and unfavorite buttons when user add route to favorite or delete this route.
        /// </summary>
        private void SetVisibilityToFavoriteIcons(bool favorite, bool unfavorite)
        {
            IsVisibleFavoriteIcon = favorite;
            IsVisibleUnFavoriteIcon = unfavorite;
        }


        #endregion
    }
}