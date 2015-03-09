using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using Windows.UI.Popups;
using Caliburn.Micro;
using Trains.Infrastructure.Infrastructure;
using Trains.Model.Entities;
using Trains.Services.Interfaces;
using TrainSearch.Entities;

namespace Trains.App.ViewModels
{
    /// <summary>
    /// Search trains in the right direction
    /// </summary>
    public class ItemPageViewModel : Screen
    {
        #region constant

        private const string IncorrectInput = "Один или оба пункта не существует, проверьте еще раз ввод";
        private const string ConectionError = "Проверьте подключение к интернету";
        private const string SearchError = "Поезда на дату отправления не найдены";
        private const string DateUpTooLater = "Поиск может производится начиная от текущего времени";
        private const string DateTooBig ="Поиск может производится только за 45 дней от текущего момента или используйте режим \"На все дни\"";
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
        #endregion

        #region constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="navigationService">Used to navigate between pages.</param>
        /// <param name="search">Used to search train schedule.</param>
        /// <param name="serializable">Used to serialization/deserialization objects.</param>
        public ItemPageViewModel(INavigationService navigationService, ISearchService search, ISerializableService serializable)
        {
            _navigationService = navigationService;
            _search = search;
            _serializable = serializable;
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
        private readonly BindableCollection<string> _variantOfSearch = new BindableCollection<string>(new[] { "Все поезда", "На все дни" });
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
        /// Keeps all stop points.
        /// </summary> 
        public static IEnumerable<CountryStopPointDataItem> AutoCompletion { get; set; }

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
        protected async override void OnActivate()
        {
            if (Parameter != null)
            {
                From = Parameter.From;
                To = Parameter.To;
            }
            _isVisibleFavoriteIcon = true;
            SelectedDate = Date[0];
            if (AutoCompletion == null)
                AutoCompletion = await _search.GetCountryStopPoint();
            LastRequests = SavedItems.LastRequests;
            FavoriteRequests = SavedItems.FavoriteRequests;
        }

        /// <summary>
        /// Searches for train schedules at a specified date in the specified mode.
        /// </summary>
        private async void Search()
        {
            if (!CheckInput()) return;
            IsTaskRun = true;
            _serializable.SerializeLastRequest(From, To, LastRequests);
            var schedule = await _search.GetTrainSchedule(From, To, GetDate());
            IsTaskRun = false;
            if (schedule == null || !schedule.Any())
                ShowMessageBox(SearchError);
            else
                _navigationService.NavigateToViewModel<SchedulePageViewModel>(schedule);
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
        }
        
        /// <summary>
        /// Saves the entered route to favorite.
        /// </summary>
        private async void AddToFavorite()
        {
            if (FavoriteRequests == null) FavoriteRequests = new List<LastRequest>();
            if (string.IsNullOrEmpty(From) || string.IsNullOrEmpty(To)) { ShowMessageBox("Одна или обе станции не введены"); return; }
            if (FavoriteRequests.Any(x => x.From == From && x.To == To)) ShowMessageBox("Данный маршрут уже присутствует");
            else
            {
                FavoriteRequests.Add(new LastRequest { From = From, To = To });
                SavedItems.FavoriteRequests = FavoriteRequests;
                await Serialize.SaveObjectToXml(FavoriteRequests, "favoriteRequests");
                ShowMessageBox("Станция добавлена!");
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
                ShowMessageBox("Ваш список пуст");
                return;
            }
            var objectToDelete = FavoriteRequests.FirstOrDefault(x => x.From == From && x.To == To);
            if (objectToDelete != null)
            {
                FavoriteRequests.Remove(objectToDelete);
                SavedItems.FavoriteRequests = FavoriteRequests;
                _serializable.SerializeObjectToXml(LastRequests, "favoriteRequests");
                ShowMessageBox("Станция удалена!");
                SetVisibilityToFavoriteIcons(true, false);
            }
            else
                ShowMessageBox("Маршрут не найден,попробуйте сначало добавить!");
        }

        /// <summary>
        /// Go to favorite routes page.
        /// </summary>
        private void GoToFavoriteList()
        {
            if (FavoriteRequests != null)
                _navigationService.NavigateToViewModel<FavoritePageViewModel>();
            else
                ShowMessageBox("Ваш список пуст!");
        }

        /// <summary>
        /// Go to help page,informations about belarussian trains icons.
        /// </summary>
        private void GoToHelpPage()
        {
            _navigationService.NavigateToViewModel<HelpPageViewModel>();
        }

        /// <summary>
        /// Check input,before will perform a search.
        /// </summary>
        private bool CheckInput()
        {
            if (IsTaskRun || CheckDate() || string.IsNullOrEmpty(From) || string.IsNullOrEmpty(To) ||
                (!AutoCompletion.Any(x => x.UniqueId.Contains(From.Split('(')[0])) || !AutoCompletion.Any(x => x.UniqueId.Contains(To.Split('(')[0]))))
            {
                ShowMessageBox(IncorrectInput);
                return false;
            }
            if (NetworkInterface.GetIsNetworkAvailable()) return true;
            ShowMessageBox(ConectionError);
            return false;
        }

        /// <summary>
        /// Check,that date in correct format.
        /// </summary>
        private bool CheckDate()
        {
            if ((Datum.Date - DateTime.Now).Days < 0)
            {
                ShowMessageBox(DateUpTooLater);
                return true;
            }
            if (Datum.Date <= DateTime.Now.AddDays(45)) return false;
            ShowMessageBox(DateTooBig);
            return true;
        }

        /// <summary>
        /// Update prompts during user input stopping point
        /// </summary>
        private void UpdateAutoSuggestions(string str)
        {
            if (FavoriteRequests != null && FavoriteRequests.Any(x => x.From == From && x.To == To))
                SetVisibilityToFavoriteIcons(false, true);
            else
                SetVisibilityToFavoriteIcons(true, false);
            AutoSuggestions = AutoCompletion.Where(x => x.UniqueId.Contains(str)).Select(x => x.UniqueId + x.Country).ToList();
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

        /// <summary>
        /// Show message box.
        /// </summary>
        private async void ShowMessageBox(string message)
        {
            var dialog = new MessageDialog(message);
            await dialog.ShowAsync();
        }

        /// <summary>
        /// Swaps From and To filds.
        /// </summary>
        public void Swap()
        {
            if (From == null || To == null) return;
            var temp = From;
            From = To;
            To = temp;
        }

        /// <summary>
        /// Check the search mode and issue the required date.
        /// </summary>
        private string GetDate()
        {
            if (SelectedDate == Date[1]) return "everyday";
            return Datum.Date.Year + "-" + Datum.Date.Month + "-" + Datum.Date.Day;
        }

        #endregion
    }
}