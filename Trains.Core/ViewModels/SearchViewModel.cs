using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cirrious.MvvmCross.ViewModels;
using Trains.Model.Entities;
using Trains.Services.Interfaces;
using Newtonsoft.Json;
using System.Net.NetworkInformation;
using Trains.Resources;
using Chance.MvvmCross.Plugins.UserInteraction;
using Cirrious.CrossCore;
using Trains.Core.Interfaces;
using Trains.Entities;
using System.Runtime.ExceptionServices;

namespace Trains.Core.ViewModels
{
    public class SearchViewModel : MvxViewModel
    {
        #region readonlyProperties

        private readonly IAppSettings _appSettings;

        private readonly IFavoriteManageService _manageFavoriteRequest;

        /// <summary>
        /// Used to search train schedule.
        /// </summary>
        private readonly ISearchService _search;

        /// <summary>
        /// Used to serialization/deserialization objects.
        /// </summary>
        private readonly ISerializableService _serializable;

        #endregion

        #region command

        public IMvxCommand SearchCommand { get; private set; }
        public IMvxCommand ClearCommand { get; private set; }
        public IMvxCommand AddToFavoriteCommand { get; private set; }
        public IMvxCommand DeleteInFavoriteCommand { get; private set; }
        public IMvxCommand GoToHelpCommand { get; private set; }
        public IMvxCommand SetLastRouteCommand { get; private set; }
        public IMvxCommand SwapCommand { get; private set; }

        #endregion

        #region ctor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="search">Used to search train schedule.</param>
        /// <param name="serializable">Used to serialization/deserialization objects.</param>
        /// <param name="checkTrain">Used to CHeck</param>
        /// <param name="manageFavoriteRequest"></param>
        public SearchViewModel(ISearchService search, ISerializableService serializable, IFavoriteManageService manageFavoriteRequest, IAppSettings appSettings)
        {
            _appSettings = appSettings;
            _search = search;
            _serializable = serializable;
            _manageFavoriteRequest = manageFavoriteRequest;

            SearchCommand = new MvxCommand(Search);
            ClearCommand = new MvxCommand(Clear);
            AddToFavoriteCommand = new MvxCommand(AddToFavorite);
            DeleteInFavoriteCommand = new MvxCommand(DeleteInFavorite);
            GoToHelpCommand = new MvxCommand(GoToHelpPage);
            SetLastRouteCommand = new MvxCommand(() => SetRequest(LastRequest));
            SwapCommand = new MvxCommand(Swap);
        }

        #endregion

        #region properties

        /// <summary>
        /// Used to display favorite icon.
        /// </summary> 
        private LastRequest _lastRequest;
        public LastRequest LastRequest
        {
            get
            {
                return _lastRequest;
            }

            set
            {
                _lastRequest = value;
                RaisePropertyChanged(() => LastRequest);
            }
        }

        /// <summary>
        /// Stores variant of search.
        /// </summary> 
        public List<string> VariantOfSearch
        {
            get
            {
                return new List<string>
                {
                    ResourceLoader.Instance.Resource.GetString("OnDay"),
                    ResourceLoader.Instance.Resource.GetString("OnAllDays")
                };
            }
        }

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
        /// Used to set code behind variant of search.
        /// </summary> 
        private string _selectedDate;
        public string SelectedVariant
        {
            get
            {
                return _selectedDate;
            }

            set
            {
                _selectedDate = value;
                RaisePropertyChanged(() => SelectedVariant);
            }
        }

        /// <summary>
        /// Used to read and set start stop point.
        /// </summary> 
        private string _from;
        public string From
        {
            get { return _from; }
            set
            {
                _from = value;
                RaisePropertyChanged(() => From);
                UpdateAutoSuggestions(From);
            }
        }

        /// <summary>
        /// Used to read and set start end point.
        /// </summary> 
        private string _to;
        public string To
        {
            get { return _to; }
            set
            {
                _to = value;
                RaisePropertyChanged(() => To);
                UpdateAutoSuggestions(To);
            }
        }

        /// <summary>
        /// Displaying last user request.
        /// </summary> 
        private List<LastRequest> _lastRequests;
        public List<LastRequest> LastRequests
        {
            get { return _lastRequests; }
            set
            {
                _lastRequests = value;
                RaisePropertyChanged(() => LastRequests);
            }
        }

        /// <summary>
        /// Keeps on what date search train.
        /// </summary> 
        private DateTimeOffset _datum = new DateTimeOffset(DateTime.Now);
        public DateTimeOffset Datum
        {
            get { return _datum; }
            set
            {
                _datum = value;
                RaisePropertyChanged(() => Datum);
            }
        }

        /// <summary>
        /// Contains stopping points satisfying user input.
        /// </summary> 
        private List<string> _autoSuggestions;
        public List<string> AutoSuggestions
        {
            get { return _autoSuggestions; }
            set
            {
                _autoSuggestions = value;
                RaisePropertyChanged(() => AutoSuggestions);
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

        /// <summary>
        /// Used for field enabled control.
        /// </summary>
        private bool _isFildEnabled = true;

        public bool IsFildEnabled
        {
            get { return _isFildEnabled; }
            set
            {
                _isFildEnabled = value;
                RaisePropertyChanged(() => IsFildEnabled);
            }
        }

		public List<string> AutoCompletion
		{
			get { return _appSettings.AutoCompletion.Select(x => x.UniqueId).ToList(); }
		}
        #endregion

        #region actions

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// Set the default parameter of some properties.
        /// </summary>
        public void Init(string param)
        {
            if (param != null)
            {
                LastRequest temp = JsonConvert.DeserializeObject<LastRequest>(param);
                From = temp.From;
                To = temp.To;
            }
            SelectedVariant = VariantOfSearch[0];
            LastRequests = _appSettings.LastRequests;
        }

        /// <summary>
        /// Searches for train schedules at a specified date in the specified mode.
        /// </summary>
        private async void Search()
        {
            if (IsTaskRun || await CheckInput()) return;
            IsTaskRun = true;
            List<Train> schedule = null;
            ExceptionDispatchInfo capturedException = null;
            try
            {
                schedule = await _search.GetTrainSchedule(_appSettings.AutoCompletion.First(x => x.UniqueId == From), _appSettings.AutoCompletion.First(x => x.UniqueId == To), Datum, SelectedVariant);
            }
            catch (Exception e)
            {
                capturedException = ExceptionDispatchInfo.Capture(e);
            }

            if(capturedException != null)
                await Mvx.Resolve<IUserInteraction>().AlertAsync(ResourceLoader.Instance.Resource.GetString("SearchError"));
            if (!schedule.Any())
                await Mvx.Resolve<IUserInteraction>().AlertAsync(ResourceLoader.Instance.Resource.GetString("TrainsNotFound"));
            else
            {
                _appSettings.LastRequestTrain = schedule;
                await SerializeDataSearch();
                await _serializable.SerializeObjectToXml(schedule, Constants.LastTrainList);
                ShowViewModel<ScheduleViewModel>(new { param = JsonConvert.SerializeObject(schedule) });
            }
            IsTaskRun = false;
        }

        private async Task SerializeDataSearch()
        {
            SerializeLastRequests();
            _appSettings.UpdatedLastRequest = new LastRequest { From = From, To = To, SelectionMode = SelectedVariant, Date = Datum };
            await _serializable.SerializeObjectToXml(_appSettings.UpdatedLastRequest, Constants.UpdateLastRequest);
        }

        private async void SerializeLastRequests()
        {
            if (LastRequests == null) LastRequests = new List<LastRequest>(3);
            if (LastRequests.Any(x => x.From == From && x.To == To)) return;
            if (LastRequests.Count == 3)
            {
                LastRequests[2] = LastRequests[1];
                LastRequests[1] = LastRequests[0];
                LastRequests[0] = new LastRequest { From = From, To = To };
            }
            else
                LastRequests.Add(new LastRequest { From = From, To = To });

            _appSettings.LastRequests = LastRequests;
            await _serializable.SerializeObjectToXml<List<LastRequest>>(LastRequests, Constants.LastRequests);
        }

        /// <summary>
        /// Swaped From and To properties.
        /// </summary>
        private void Swap()
        {
            var tmp = From;
            From = To;
            To = tmp;
        }

        /// <summary>
        /// Sets the user-selected route.
        /// </summary>
        private void SetRequest(LastRequest lastRequest)
        {
            From = lastRequest.From;
            To = lastRequest.To;
        }

        /// <summary>
        /// Сlears the field From and To.
        /// </summary>
        private void Clear()
        {
            From = String.Empty;
            To = String.Empty;
            SetVisibilityToFavoriteIcons(false, false);
        }

        /// <summary>
        /// Saves the entered route to favorite.
        /// </summary>
        private async void AddToFavorite()
        {
            if (await Task.Run(() => _manageFavoriteRequest.AddToFavorite(From, To)))
                SetVisibilityToFavoriteIcons(false, true);
        }

        /// <summary>
        /// Deletes the entered route visas favorite.
        /// </summary>
        private async void DeleteInFavorite()
        {
            if (await Task.Run(() => _manageFavoriteRequest.DeleteRoute(From, To)))
                SetVisibilityToFavoriteIcons(true, false);
        }

        /// <summary>
        /// Go to help page,informations about belarussian trains icons.
        /// </summary>
        private void GoToHelpPage()
        {
            ShowViewModel<HelpViewModel>();
        }

        /// <summary>
        /// Update prompts during user input stopping point
        /// </summary>
        private void UpdateAutoSuggestions(string str)
        {
            if (string.IsNullOrEmpty(str)) return;
            AutoSuggestions = _appSettings.AutoCompletion.Where(x => x.UniqueId.IndexOf(str, StringComparison.OrdinalIgnoreCase) >= 0).Select(x => x.UniqueId).ToList();
            if (AutoSuggestions.Count != 1 || AutoSuggestions[0] != str) return;
            AutoSuggestions.Clear();

            if (CheckFavorite())
                SetVisibilityToFavoriteIcons(true, false);
            else
                SetVisibilityToFavoriteIcons(false, true);
        }

        /// <summary>
        /// Change visibility of favorite and unfavorite buttons when user add route to favorite or delete this route.
        /// </summary>
        private void SetVisibilityToFavoriteIcons(bool favorite, bool unfavorite)
        {
            IsVisibleFavoriteIcon = favorite;
            IsVisibleUnFavoriteIcon = unfavorite;
        }

        public async Task<bool> CheckInput()
        {
            if ((Datum.Date - DateTime.Now).Days < 0)
            {
                await Mvx.Resolve<IUserInteraction>().AlertAsync(ResourceLoader.Instance.Resource.GetString("DateUpTooLater"));
                return true;
            }
            if (Datum.Date > DateTime.Now.AddDays(45))
            {
                await Mvx.Resolve<IUserInteraction>().AlertAsync(ResourceLoader.Instance.Resource.GetString("DateTooBig"));
                return true;
            }

            if (String.IsNullOrEmpty(From) || String.IsNullOrEmpty(To) ||
                !(_appSettings.AutoCompletion.Any(x => x.UniqueId == From) &&
                  _appSettings.AutoCompletion.Any(x => x.UniqueId == To)))
            {
                await Mvx.Resolve<IUserInteraction>().AlertAsync(ResourceLoader.Instance.Resource.GetString("IncorrectInput"));
                return true;
            }

            if (NetworkInterface.GetIsNetworkAvailable()) return false;
            await Mvx.Resolve<IUserInteraction>().AlertAsync(ResourceLoader.Instance.Resource.GetString("ConectionError"));
            return true;
        }

        public bool CheckFavorite()
        {
            if (_appSettings.FavoriteRequests == null || !_appSettings.FavoriteRequests.Any()) return true;
            return !string.IsNullOrEmpty(From) && !string.IsNullOrEmpty(To) && !_appSettings.FavoriteRequests.Any(x => x.From == From && x.To == To);
        }

        #endregion
    }
}
