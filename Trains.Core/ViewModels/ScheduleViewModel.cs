using System.Collections.Generic;
using Cirrious.MvvmCross.ViewModels;
using Newtonsoft.Json;
using Trains.Core.Interfaces;
using Trains.Entities;
using System.Linq;
using System.Threading.Tasks;
using Trains.Core;

namespace Trains.Core.ViewModels
{
    public class ScheduleViewModel : MvxViewModel
    {
        #region readonlyProperty

        private readonly IAppSettings _appSettings;
        private readonly IFavoriteManageService _manageFavoriteRequest;
        private readonly IAnalytics _analytics;

        #endregion

        #region command

        public IMvxCommand GoToHelpPageCommand { get; private set; }
        public MvxCommand<Train> SelectTrainCommand { get; private set; }
        public IMvxCommand AddToFavoriteCommand { get; private set; }
        public IMvxCommand DeleteInFavoriteCommand { get; private set; }

        #endregion

        #region ctor

        public ScheduleViewModel(IAppSettings appSettings, IFavoriteManageService manageFavoriteRequest,IAnalytics analytics)
        {
            _manageFavoriteRequest = manageFavoriteRequest;
            _appSettings = appSettings;
            _analytics = analytics;

            AddToFavoriteCommand = new MvxCommand(AddToFavorite);
            DeleteInFavoriteCommand = new MvxCommand(DeleteInFavorite);
            GoToHelpPageCommand = new MvxCommand(GoToHelpPage);
            SelectTrainCommand = new MvxCommand<Train>(ClickItem);
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

        /// <summary>
        /// Ñontains information on all trains on the route selected by the user.
        /// </summary> 
        public IEnumerable<Train> Trains { get; set; }

        public string Request { get; set; }

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
            SetManageFavoriteButton();
        }

        /// <summary>
        /// Invoked when the user selects his train of interest.
        /// Go to the information page which show additional information about this train.
        /// </summary>
        /// <param name="train">Data that describes user-selected train(prices,seats,stop points and other)</param>
        private void ClickItem(Train train)
        {
            if (train == null) return;
            ShowViewModel<InformationViewModel>(new { param = JsonConvert.SerializeObject(train) });
        }

        /// <summary>
        /// Go to saved by user routes page.
        /// </summary>
        private void GoToHelpPage()
        {
            ShowViewModel<HelpViewModel>();
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
        private void AddToFavorite()
        {
            if (_manageFavoriteRequest.AddToFavorite(_appSettings.UpdatedLastRequest.From, _appSettings.UpdatedLastRequest.To))
            {
                SetVisibilityToFavoriteIcons(false, true);
                _analytics.SentEvent(Constants.AddToFavorite);

            }
        }

        /// <summary>
        /// Deletes the entered route visas favorite.
        /// </summary>
        private void DeleteInFavorite()
        {
            if (_manageFavoriteRequest.DeleteRoute(_appSettings.UpdatedLastRequest.From, _appSettings.UpdatedLastRequest.To))
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