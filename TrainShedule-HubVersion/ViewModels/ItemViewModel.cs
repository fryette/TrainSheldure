using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;
using Trains.Infrastructure.Infrastructure;
using Trains.Model.Entities;
using Trains.Services.Interfaces;
using Trains.Services.Tools;

namespace Trains.App.ViewModels
{
    /// <summary>
    /// Search trains in the right direction
    /// </summary>
    public class ItemViewModel : Screen
    {
        #region constant

        private const string SearchError = "Поезда на дату отправления не найдены";
        private const string ThisRouteIsPresent = "Данный маршрут уже присутствует";
        private const string OneOrMoreStopPointIsInCorrect = "Одна или обе станции не введены";
        private const string FavoriteString = "favoriteRequests";
        private const string RouteIsAddedToFavorite = "Станция добавлена!";
        private const string FavoriteListIsEmpthy = "Ваш список пуст";
        private const string RouteIsDeletedInFavorite = "Станция удалена!";
        private const string RouteIsIncorect = "Маршрут не найден,проверьте поля ввода станций!";
        private const string UpdateLastRequestString = "updateLastRequst";

        #endregion

        #region readonlyProperties

        /// <summary>
        /// Used to navigate between pages.
        /// </summary>
        private readonly INavigationService _navigationService;

        /// <summary>
        /// Used to search train schedule.
        /// </summary>
        private readonly ISearchService _search;

        /// <summary>
        /// Used to serialization/deserialization objects.
        /// </summary>
        private readonly ISerializableService _serializable;

        /// <summary>
        /// Used to navigate between pages.
        /// </summary>
        private readonly ICheckTrainService _checkTrain;

        #endregion

        #region constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="navigationService">Used to navigate between pages.</param>
        /// <param name="search">Used to search train schedule.</param>
        /// <param name="serializable">Used to serialization/deserialization objects.</param>
        /// <param name="checkTrain">Used to CHeck</param>
        public ItemViewModel(INavigationService navigationService, ISearchService search, ISerializableService serializable, ICheckTrainService checkTrain)
        {
            _navigationService = navigationService;
            _search = search;
            _serializable = serializable;
            _checkTrain = checkTrain;
        }
        #endregion

        #region properties

        /// <summary>
        /// Used to display the completed fields from and to.
        /// </summary> 
        public LastRequest Parameter { get; set; }

        /// <summary>
        /// Stores variant of search.
        /// </summary> 
        private readonly BindableCollection<string> _variantOfSearch = new BindableCollection<string>(new[] { "На дату", "На все дни" });
        public BindableCollection<string> Date
        {
            get { return _variantOfSearch; }
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
                NotifyOfPropertyChange(() => IsVisibleFavoriteIcon);
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
                NotifyOfPropertyChange(() => IsVisibleUnFavoriteIcon);
            }
        }

        /// <summary>
        /// Used to set code behind variant of search.
        /// </summary> 
        private string _selectedDate;
        public string SelectedDate
        {
            get
            {
                return _selectedDate;
            }

            set
            {
                _selectedDate = value;
                NotifyOfPropertyChange(() => SelectedDate);
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
                NotifyOfPropertyChange(() => From);
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
                NotifyOfPropertyChange(() => To);
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
                NotifyOfPropertyChange(() => LastRequests);
            }
        }

        /// <summary>
        /// Keeps saved by user routes.
        /// </summary> 
        private List<LastRequest> _favoriteRequests;
        public List<LastRequest> FavoriteRequests
        {
            get { return _favoriteRequests; }
            set
            {
                _favoriteRequests = value;
                NotifyOfPropertyChange(() => FavoriteRequests);
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
                NotifyOfPropertyChange(() => Datum);
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
                NotifyOfPropertyChange(() => AutoSuggestions);
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
                NotifyOfPropertyChange(() => IsTaskRun);
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
                NotifyOfPropertyChange(() => IsFildEnabled);
            }
        }
        #endregion

        #region actions

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// Set the default parameter of some properties.
        /// </summary>
        protected override void OnActivate()
        {
            if (Parameter != null)
            {
                From = Parameter.From;
                To = Parameter.To;
            }
            SelectedDate = Date[0];
            LastRequests = SavedItems.LastRequests;
            FavoriteRequests = SavedItems.FavoriteRequests;
        }

        /// <summary>
        /// Searches for train schedules at a specified date in the specified mode.
        /// </summary>
        private async void Search()
        {
            if (await CheckInput()) return;
            IsTaskRun = true;
            await Task.Run(() => SerializeDataSearch());
            var schedule = await Task.Run(() => _search.GetTrainSchedule(From, To, ToolHelper.GetDate(Datum, SelectedDate)));
            if (schedule == null || !schedule.Any())
                ToolHelper.ShowMessageBox(SearchError);
            else
                _navigationService.NavigateToViewModel<ScheduleViewModel>(schedule);
            IsTaskRun = false;
        }

        private void SerializeDataSearch()
        {
            SavedItems.LastRequests = _serializable.SerializeLastRequest(From, To, LastRequests);
            SavedItems.UpdatedLastRequest = new LastRequest { From = From, To = To, SelectionMode = SelectedDate, Date = Datum };
            _serializable.SerializeObjectToXml(SavedItems.UpdatedLastRequest, UpdateLastRequestString);
        }

        private async Task<bool> CheckInput()
        {
            if (IsTaskRun) return true;
            IsTaskRun = true;
            var checkInputError = await Task.Run(() => _checkTrain.CheckInput(From, To, Datum));
            if (checkInputError == null) return false;
            ToolHelper.ShowMessageBox(checkInputError);
            IsTaskRun = false;
            return true;
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
        /// Close input keyboard,when user click on promt.
        /// </summary>
        private void SuggestionChosen()
        {
            IsFildEnabled = false;
            IsFildEnabled = true;
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
            if (string.IsNullOrEmpty(From) || string.IsNullOrEmpty(To))
            {
                ToolHelper.ShowMessageBox(OneOrMoreStopPointIsInCorrect);
                return;
            }
            if (SavedItems.FavoriteRequests == null) SavedItems.FavoriteRequests = new List<LastRequest>();
            if (SavedItems.FavoriteRequests.Any(x => x.From == From && x.To == To))
                ToolHelper.ShowMessageBox(ThisRouteIsPresent);
            else
            {
                SavedItems.FavoriteRequests.Add(new LastRequest { From = From, To = To });
                FavoriteRequests = SavedItems.FavoriteRequests;
                await Serialize.SaveObjectToXml(FavoriteRequests, FavoriteString);
                ToolHelper.ShowMessageBox(RouteIsAddedToFavorite);
                SetVisibilityToFavoriteIcons(false, true);
            }
        }

        /// <summary>
        /// Deletes the entered route visas favorite.
        /// </summary>
        private void DeleteInFavorite()
        {
            if (FavoriteRequests == null)
            {
                ToolHelper.ShowMessageBox(FavoriteListIsEmpthy);
                return;
            }
            var objectToDelete = FavoriteRequests.FirstOrDefault(x => x.From == From && x.To == To);
            if (objectToDelete != null)
            {
                FavoriteRequests.Remove(objectToDelete);
                SavedItems.FavoriteRequests = FavoriteRequests;
                _serializable.SerializeObjectToXml(LastRequests, FavoriteString);
                ToolHelper.ShowMessageBox(RouteIsDeletedInFavorite);
                SetVisibilityToFavoriteIcons(true, false);
            }
            else
                ToolHelper.ShowMessageBox(RouteIsIncorect);
        }

        /// <summary>
        /// Go to help page,informations about belarussian trains icons.
        /// </summary>
        private void GoToHelpPage()
        {
            _navigationService.NavigateToViewModel<HelpViewModel>();
        }

        /// <summary>
        /// Update prompts during user input stopping point
        /// </summary>
        private void UpdateAutoSuggestions(string str)
        {
            if (string.IsNullOrEmpty(str)) return;
            if (_checkTrain.CheckFavorite(From, To))
                SetVisibilityToFavoriteIcons(true, false);
            else
                SetVisibilityToFavoriteIcons(false, true);
            AutoSuggestions = SavedItems.AutoCompletion.Where(x => x.UniqueId.Contains(str)).Select(x => x.UniqueId).ToList();
            if (AutoSuggestions.Count != 1 || AutoSuggestions[0] != str) return;
            AutoSuggestions.Clear();
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