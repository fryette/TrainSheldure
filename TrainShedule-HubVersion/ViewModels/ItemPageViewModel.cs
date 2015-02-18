using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Caliburn.Micro;
using TrainShedule_HubVersion.DataModel;
using TrainShedule_HubVersion.Infrastructure;

namespace TrainShedule_HubVersion.ViewModels
{
    public class ItemPageViewModel : Screen
    {
        public MenuDataItem Parameter { get; set; }

        private readonly INavigationService _navigationService;

        public ItemPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
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
                AutoSuggestions = AutoCompletion.Where(x => x.Contains(From));
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
                AutoSuggestions = AutoCompletion.Where(x => x.Contains(To));
            }
        }

        private DateTimeOffset _datum= new DateTimeOffset(DateTime.Now);
        public DateTimeOffset Datum
        {
            get { return _datum; }
            set
            {
                _datum = value;
                NotifyOfPropertyChange(() => Datum);
            }
        }
        public IEnumerable<string> AutoCompletion { get; set; }
        private IEnumerable<string> _autoSuggestions;
        public IEnumerable<string> AutoSuggestions
        {
            get { return _autoSuggestions; }
            set
            {
                _autoSuggestions = value;
                NotifyOfPropertyChange(() => AutoSuggestions);
            }
        }
        private Visibility _visibilityProgressBar;
        public Visibility VisibilityProgressBar
        {
            get { return _visibilityProgressBar; }
            set
            {
                _visibilityProgressBar = value;
                NotifyOfPropertyChange(() => VisibilityProgressBar);
            }
        }

        protected async override void OnActivate()
        {
            Title = Parameter.Title;
            VisibilityProgressBar = Visibility.Collapsed;
            if (AutoCompletion == null)
                AutoCompletion = await Serialize.ReadObjectFromXmlFileAsync<string>("autocompletion");
            if (AutoCompletion == null)
                ShowMessageBox("Обновите станции,это делается всего один раз.");
        }
        private static async void ShowMessageBox(string message)
        {
            var messageDialog = new MessageDialog(message);
            await messageDialog.ShowAsync();
        }
        public void Swap()
        {
            var temp = From;
            From = To;
            To = temp;
        }
        private async void Search()
        {
            if (AutoCompletion == null || From == String.Empty || To == String.Empty ||
                (!AutoCompletion.Contains(From) || !AutoCompletion.Contains(To)))
            {
                ShowMessageBox("Один или оба пункта не существует, проверьте еще раз или обновите станции");
                return;
            }
            VisibilityProgressBar = Visibility.Visible;
            var schedule = await TrainGrabber.GetTrainSchedule(From, To, GetDate(), Parameter.Title, Parameter.IsEconom, Parameter.SpecialSearch);
            _navigationService.NavigateToViewModel<SchedulePageViewModel>(schedule);
            VisibilityProgressBar = Visibility.Collapsed;
        }
        private string GetDate()
        {
            return Datum.Date.Year + "-" + Datum.Date.Month + "-" + Datum.Date.Day;
        }
        private async void UpdateTrainStop()
        {
            VisibilityProgressBar = Visibility.Visible;
            AutoCompletion = await Task.Run(() => TrainPointsGrabber.GetTrainPoints());
            if (AutoCompletion != null)
            {
                await Serialize.SaveObjectToXml(new List<string>(AutoCompletion), "autocompletion");
                ShowMessageBox("Обновление прошло успешно");
            }
            else
            {
                ShowMessageBox("Сбой,попробуйте позже или проверьте связь интернет");
            }
            VisibilityProgressBar = Visibility.Collapsed;
        }
    }
}
