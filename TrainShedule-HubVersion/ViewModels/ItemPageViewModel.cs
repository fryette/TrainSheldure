using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using Windows.System;
using Windows.UI.Popups;
using Caliburn.Micro;
using Trains.Services.Interfaces;
using TrainSearch.Entities;
using TrainSearch.Infrastructure;

namespace Trains.App.ViewModels
{
    public class ItemPageViewModel : Screen
    {

        public MenuDataItem Parameter { get; set; }

        private readonly INavigationService _navigationService;
        private readonly ISearch _search;
        private readonly ISerializable _serializable;

        #region constructors
        public ItemPageViewModel(INavigationService navigationService, ISearch item, ISerializable serializable)
        {
            _navigationService = navigationService;
            _search = item;
            _serializable = serializable;
        }
        #endregion

        #region properties
        private readonly BindableCollection<string> _variantOfDate = new BindableCollection<string>(new[] { "Все поезда", "На все дни" });
        public BindableCollection<string> Date
        {
            get { return _variantOfDate; }
        }

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

        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                NotifyOfPropertyChange(() => Title);
            }
        }

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

        public static IEnumerable<CountryStopPointDataItem> AutoCompletion { get; set; }

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
        protected async override void OnActivate()
        {
            SelectedDate = Date[0];
            Title = Parameter.Title;
            if (AutoCompletion == null)
                AutoCompletion = await _search.GetCountryStopPoint();
            LastRequests = await _serializable.GetLastRequests("lastRequests");
            FavoriteRequests = await _serializable.GetLastRequests("favoriteRequests");
            if (Parameter.Title == "Аэропорт")
                From = "Национальный аэропорт «Минск» (Беларусь)";
            if (Parameter.From == null) return;
            From = Parameter.From;
            To = Parameter.To;
        }

        private static async void ShowMessageBox(string message)
        {
            var messageDialog = new MessageDialog(message);
            await messageDialog.ShowAsync();
        }

        public void Swap()
        {
            if (From == null || To == null) return;
            var temp = From;
            From = To;
            To = temp;
        }

        private async void Search()
        {
            if (CheckDate()) return;
            if (AutoCompletion == null || string.IsNullOrEmpty(From) || string.IsNullOrEmpty(To) ||
                (!AutoCompletion.Any(x => x.UniqueId.Contains(From.Split('(')[0])) || !AutoCompletion.Any(x => x.UniqueId.Contains(To.Split('(')[0]))))
            {
                ShowMessageBox("Один или оба пункта не существует, проверьте еще раз или обновите станции");
                return;
            }
            if (IsTaskRun) return;
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                IsTaskRun = true;
                _serializable.SerializeLastRequest(From, To, LastRequests);
                var schedule = await _search.GetTrainSchedule(Parameter.IsEconom, Parameter.SpecialSearch, From, To, GetDate(), Parameter.Title);
                IsTaskRun = false;
                if (schedule == null || !schedule.Any())
                    ShowMessageBox("Поезда на дату отправления не найдены");
                else
                    _navigationService.NavigateToViewModel<SchedulePageViewModel>(schedule);
            }
            else ShowMessageBox("Проверьте подключение к сети интернет");
        }

        private bool CheckDate()
        {
            if ((Datum.Date - DateTime.Now).Days < 0)
            {
                ShowMessageBox("Поиск может производится начиная от текущего времени");
                return true;
            }
            if (Datum.Date <= DateTime.Now.AddDays(45)) return false;
            ShowMessageBox("Поиск может производится только за 45 дней от текущего момента или используйте режим \"На все дни\"");
            return true;
        }

        private string GetDate()
        {
            if (SelectedDate == Date[1]) return "everyday";
            return Datum.Date.Year + "-" + Datum.Date.Month + "-" + Datum.Date.Day;
        }

        private void SetRequest(LastRequest lastRequest)
        {
            From = lastRequest.From;
            To = lastRequest.To;
        }

        private void UpdateAutoSuggestions(string str)
        {
            AutoSuggestions = AutoCompletion.Where(x => x.UniqueId.Contains(str)).Select(x => x.UniqueId + x.Country).ToList();
            if (AutoSuggestions.Count != 1 || AutoSuggestions[0] != str) return;
            AutoSuggestions.Clear();
        }

        private void SuggestionChosen()
        {
            IsFildEnabled = false;
            IsFildEnabled = true;
        }

        private void Clear()
        {
            From = String.Empty;
            To = String.Empty;
        }

        private async void AddToFavorite()
        {
            if (FavoriteRequests == null) FavoriteRequests = new List<LastRequest>();
            if (string.IsNullOrEmpty(From) || string.IsNullOrEmpty(To)) { ShowMessageBox("Одна или обе станции не введены"); return; }
            if (FavoriteRequests.Any(x => x.From == From && x.To == To)) ShowMessageBox("Данный маршрут уже присутствует");
            else
            {
                FavoriteRequests.Add(new LastRequest { From = From, To = To });
                await Serialize.SaveObjectToXml(FavoriteRequests, "favoriteRequests");
                ShowMessageBox("Успешно добавлен!");
            }
        }

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
                _serializable.SerializeObjectToXml(LastRequests, "favoriteRequests");
                ShowMessageBox("Успешно удален!");
            }
            else
                ShowMessageBox("Маршрут не найден,попробуйте сначало добавить!");
        }

        private void GoToFavoriteList()
        {
            if (FavoriteRequests != null)
                _navigationService.NavigateToViewModel<FavoritePageViewModel>(Parameter);
            else
                ShowMessageBox("Ваш список пуст!");
        }

        private async void HyperlinkClick()
        {
            await Launcher.LaunchUriAsync(new Uri("http://www.windowsphone.com/s?appid=9a0879a6-0764-4e99-87d7-4c1c33f2d78e"));
        }

        private void GoToHelpPage()
        {
            _navigationService.NavigateToViewModel<HelpPageViewModel>();
        }
        #endregion
    }
}