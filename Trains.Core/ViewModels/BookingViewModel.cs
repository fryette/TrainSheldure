using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Chance.MvvmCross.Plugins.UserInteraction;
using Cirrious.MvvmCross.ViewModels;
using Newtonsoft.Json;
using Trains.Core.Extensions;
using Trains.Core.Interfaces;
using Trains.Core.Services.Interfaces;
using Trains.Entities;
using Trains.Model.Entities;

namespace Trains.Core.ViewModels
{
    public class BookingViewModel : MvxViewModel
    {
        #region readonlyProperties

        private readonly IAppSettings _appSettings;
        private readonly IHttpService _httpService;
        private readonly IUserInteraction _userInteraction;
        #endregion

        #region command

        public IMvxCommand SendTicketRequestCommand { get; private set; }

        #endregion

        #region ctor

        public BookingViewModel(IAppSettings appSettings, IHttpService httpService, IUserInteraction userInteraction)
        {
            _appSettings = appSettings;
            _httpService = httpService;
            _userInteraction = userInteraction;

            SendTicketRequestCommand = new MvxCommand(SendTicketRequest);
        }

        #endregion

        private string _from;
        public string From
        {
            get { return _from; }
            set
            {
                _from = value;
                RaisePropertyChanged(() => From);
            }
        }

        private string _to;
        public string To
        {
            get { return _to; }
            set
            {
                _to = value;
                RaisePropertyChanged(() => To);
            }
        }

        private string _trainNumber;
        public string TrainNumber
        {
            get { return _trainNumber; }
            set
            {
                _trainNumber = value;
                RaisePropertyChanged(() => TrainNumber);
            }
        }

        private DateTime _departureTime;
        public DateTime DepartureTime
        {
            get { return _departureTime; }
            set
            {
                _departureTime = value;
                RaisePropertyChanged(() => DepartureTime);
            }
        }

        private string _email;
        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                RaisePropertyChanged(() => Email);
            }
        }

        private string _fullName;
        public string FullName
        {
            get { return _fullName; }
            set
            {
                _fullName = value;
                RaisePropertyChanged(() => FullName);
            }
        }

        private string _phoneNumber;
        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set
            {
                _phoneNumber = value;
                RaisePropertyChanged(() => PhoneNumber);
            }
        }

        public List<string> TypeOfPlace { get; } = new List<string>
        {
                "Л - 2-местный",
                "Лу - 2-местный с услугами",
                "М - мягкий",
                "К - купейный",
                "Ку - купейный с услугами",
                "П - плацкартный",
                "Пу - плацкартный с услугами",
                "О - общий",
                "С - межобластной, сидячий"
        };

        private string _selectedTypeOfPlace;
        public string SelectedTypeOfPlace
        {
            get
            {
                return _selectedTypeOfPlace;
            }

            set
            {
                _selectedTypeOfPlace = value;
                RaisePropertyChanged(() => SelectedTypeOfPlace);
            }
        }
        public List<string> Cityes { get; set; }

        private string _selectedCity;
        public string SelectedCity
        {
            get
            {
                return _selectedCity;
            }

            set
            {
                _selectedCity = value;
                RaisePropertyChanged(() => SelectedCity);
            }
        }

        public List<int> NumberOfTickets { get; } = new List<int>
        {
                1,2,3,4
        }; 

        private int _selectedNumberOfTickets;
        public int SelectedNumberOfTickets
        {
            get
            {
                return _selectedNumberOfTickets;
            }

            set
            {
                _selectedNumberOfTickets = value;
                RaisePropertyChanged(() => SelectedNumberOfTickets);
            }
        }

        public void Init(string param)
        {
            var train = JsonConvert.DeserializeObject<Train>(param);
            From = _appSettings.UpdatedLastRequest.Route.From;
            To = _appSettings.UpdatedLastRequest.Route.To;
            TrainNumber = train.City.Split(' ')[0].Substring(0, 3);
            DepartureTime = train.StartTime;
            SelectedTypeOfPlace = TypeOfPlace.FirstOrDefault();
            SelectedNumberOfTickets = NumberOfTickets.FirstOrDefault();
            Cityes = new List<string>(_appSettings.Tickets.Select(x=>x.Name));
            SelectedCity = Cityes.FirstOrDefault();
        }

        public async void SendTicketRequest()
        {
            if (!await CheckInput()) return;
            var values = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("textValue(kodd)",""),
                new KeyValuePair<string, string>("textValue(poezd_ii)",""),
                new KeyValuePair<string, string>("textValue(tip_vag_ii)",""),
                new KeyValuePair<string, string>("textValue(kol_m_ii)",""),
                new KeyValuePair<string, string>("textValue(dat_o_ii)",""),
                new KeyValuePair<string, string>("textValue(tel)",PhoneNumber),
                new KeyValuePair<string, string>("textValue(hid)","-1"),
                new KeyValuePair<string, string>("textValue(dostavka)","0"),
                new KeyValuePair<string, string>("textValue(kol_mest)",SelectedNumberOfTickets.ToString()),
                new KeyValuePair<string, string>("textValue(dat_o)", DepartureTime.ToString("dd.mm.yyyy ")),
                new KeyValuePair<string, string>("textValue(poezd)", TrainNumber),
                new KeyValuePair<string, string>("textValue(email)", Email),
                new KeyValuePair<string, string>("textValue(f_zakaz)", FullName),
                new KeyValuePair<string, string>("textValue(nsto)", From),
                new KeyValuePair<string, string>("textValue(nstn)", To),
                new KeyValuePair<string, string>("send_z1", "Отправить заявку"),
                new KeyValuePair<string, string>("textValue(tip_vag)", SelectedTypeOfPlace.Split(' ')[0])
            };

            var responseString = await _httpService.Post(new Uri("http://www.brestrw.by/zpdbrnv/zayavka.do"), values,
                new Windows1251());
        }

        private async Task<bool> CheckInput()
        {
            if (DepartureTime < DateTime.Now)
            {
                await _userInteraction.AlertAsync("Поезд уехал с указаной вами станции.");
                return false;
            }

            if (FullName == null || FullName.Split(' ').Count() < 3)
            {
                await _userInteraction.AlertAsync("Проверьте ввод поля \"ФИО пассажира\"");
                return false;
            }
            if (Email == null || !IsEmailValid())
            {
                await _userInteraction.AlertAsync("Проверьте правильность email");
                return false;
            }

            if (PhoneNumber == null || PhoneNumber.Length < 5)
            {
                await _userInteraction.AlertAsync("Проверьте правильность телефона");
                return false;
            }

            return true;
        }
        public bool IsEmailValid()
        {
            const string pattern = "[.\\-_a-z0-9]+@([a-z0-9][\\-a-z0-9]+\\.)+[a-z]{2,6}";
            var isMatch = Regex.Match(Email, pattern, RegexOptions.IgnoreCase);

            return isMatch.Success;
        }
    }
}