﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Caliburn.Micro;
using TrainShedule_HubVersion.DataModel;
using TrainShedule_HubVersion.Entities;
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
        public static IEnumerable<string> AutoCompletion { get; set; }
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

        protected async override void OnActivate()
        {
            Title = Parameter.Title;
            if (AutoCompletion == null)
                AutoCompletion = await Serialize.ReadObjectFromXmlFileAsync<string>("autocompletion");
            if (AutoCompletion == null)
            {
                ShowMessageBox("Обновите станции,это делается всего один раз.");
                return;
            }
            LastRequests = (List<LastRequest>)await Serialize.ReadObjectFromXmlFileAsync<LastRequest>("lastRequests");
            if (Parameter.Title == "Аэропорт")
                From = "Национальный аэропорт «Минск»";
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
            if (IsTaskRun) return;
            IsTaskRun = true;
            SerializeLastReques();
            var schedule = await TrainGrabber.GetTrainSchedule(From, To, GetDate(), Parameter.Title, Parameter.IsEconom, Parameter.SpecialSearch);
            _navigationService.NavigateToViewModel<SchedulePageViewModel>(schedule);
            IsTaskRun = false;
        }
        private string GetDate()
        {
            return Datum.Date.Year + "-" + Datum.Date.Month + "-" + Datum.Date.Day;
        }
        private async void UpdateTrainStop()
        {
            if(IsTaskRun)return;
            IsTaskRun = true;
            AutoCompletion = await Task.Run(() => TrainPointsGrabber.GetTrainPoints());
            if (AutoCompletion != null)
            {
                AutoCompletion = AutoCompletion.Distinct();
                await Serialize.SaveObjectToXml(new List<string>(AutoCompletion), "autocompletion");
                ShowMessageBox("Обновление прошло успешно");
            }
            else
            {
                ShowMessageBox("Сбой,попробуйте позже или проверьте связь интернет");
            }
            IsTaskRun = false;
        }

        private async void SerializeLastReques()
        {
            if (LastRequests == null) LastRequests = new List<LastRequest>();
            if (LastRequests.Any(x => x.From == From && x.To == To)) return;
            if (LastRequests.Count < 3)
            {
                LastRequests.Add(new LastRequest
                {
                    From = From,
                    To = To
                });
            }
            else
                LastRequests[2] = new LastRequest
                {
                    From = From,
                    To = To
                };
            LastRequests.Reverse();
            await Serialize.SaveObjectToXml(LastRequests, "lastRequests");
        }

        private void SetRequest(LastRequest lastRequest)
        {
            From = lastRequest.From;
            To = lastRequest.To;
        }

        private void UpdateAutoSuggestions(string str)
        {
            if (AutoCompletion == null)
            {
                ShowMessageBox("Обновите станции!");
                return;
            }
            var temp = AutoCompletion.Where(x => x.Contains(str)).ToList();
            AutoSuggestions = temp.Count() == 1 ? temp[0] == str ? null : temp : temp;
        }

    }
}
