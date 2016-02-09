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
using Trains.Core.Resources;
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
        private readonly ISerializableService _serializableService;
        #endregion

        #region command

        public IMvxCommand SendTicketRequestCommand { get; private set; }
        public MvxCommand<AdditionalTicketParameter> AdditionalParameterSelectedCommand { get; private set; }

        #endregion

        #region ctor

        public BookingViewModel(IAppSettings appSettings, IHttpService httpService, IUserInteraction userInteraction, ISerializableService serializableService)
        {
            _appSettings = appSettings;
            _httpService = httpService;
            _userInteraction = userInteraction;
            _serializableService = serializableService;

            AdditionalParameterSelectedCommand = new MvxCommand<AdditionalTicketParameter>(AdditionalParameterSelected);
            SendTicketRequestCommand = new MvxCommand(SendTicketRequest);
        }

        #endregion

        #region properties

        private List<AdditionalTicketParameter> _additionalTicketParameter;
        public List<AdditionalTicketParameter> AdditionalTicketParameters
        {
            get { return _additionalTicketParameter; }
            set
            {
                _additionalTicketParameter = value;
                RaisePropertyChanged(() => AdditionalTicketParameters);
            }
        }

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

        private DateTimeOffset _departureDate;
        public DateTimeOffset DepartureDate
        {
            get { return _departureDate; }
            set
            {
                _departureDate = value;
                RaisePropertyChanged(() => DepartureDate);
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

        #endregion

        public void Init(string param)
        {
            var train = JsonConvert.DeserializeObject<Train>(param);
            From = _appSettings.UpdatedLastRequest.Route.From;
            To = _appSettings.UpdatedLastRequest.Route.To;
            TrainNumber = train.City.Split(' ')[0].Substring(0, 3);
            DepartureDate = train.StartTime;
            SelectedTypeOfPlace = TypeOfPlace.FirstOrDefault();
            SelectedNumberOfTickets = NumberOfTickets.FirstOrDefault();
            Cityes = new List<string>(_appSettings.Tickets.Select(x => x.Name));
            AdditionalTicketParameters = new List<AdditionalTicketParameter>
            {
                new AdditionalTicketParameter {Parameter = "Кроме боковых мест"},
                new AdditionalTicketParameter {Parameter = "В начале и конце вагона не предлагать"},
                new AdditionalTicketParameter {Parameter = "В разных купе не предлагать"},
                new AdditionalTicketParameter {Parameter = "При отсутствии плацкартных мест предложить купейные"},
                new AdditionalTicketParameter {Parameter = "При отсутствии купейных мест предложить плацкартные"},
                new AdditionalTicketParameter {Parameter = "Хотя бы одно нижнее"},
                new AdditionalTicketParameter {Parameter = "При отсутствии мест на данный поезд предложить другой"}
            };

            if (_appSettings.PersonalInformation != null)
            {
                Email = _appSettings.PersonalInformation.Email;
                PhoneNumber = _appSettings.PersonalInformation.PhoneNumber;
                SelectedCity = _appSettings.PersonalInformation.City;
                FullName = _appSettings.PersonalInformation.FullName;
            }
            else
            {
                SelectedCity = Cityes.First();
            }
        }

        public async void SendTicketRequest()
        {
            if (!await CheckInput()) return;

            var values = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(Defines.Ticket.Kodd,string.Empty),
                new KeyValuePair<string, string>(Defines.Ticket.TrainBack,string.Empty),
                new KeyValuePair<string, string>(Defines.Ticket.TypeOfCarriageBack,string.Empty),
                new KeyValuePair<string, string>(Defines.Ticket.NumberOfSeatsBack,string.Empty),
                new KeyValuePair<string, string>(Defines.Ticket.DepartureDateBack,string.Empty),
                new KeyValuePair<string, string>(Defines.Ticket.PhoneNumber,PhoneNumber),
                new KeyValuePair<string, string>(Defines.Ticket.Hid,Defines.Ticket.HidValue),
                new KeyValuePair<string, string>(Defines.Ticket.Delivery,Defines.Ticket.DeliveryValue),
                new KeyValuePair<string, string>(Defines.Ticket.NumberOfTickets,SelectedNumberOfTickets.ToString()),
                new KeyValuePair<string, string>(Defines.Ticket.DepartureDate, DepartureDate.ToString(Defines.Ticket.DateFormat)),
                new KeyValuePair<string, string>(Defines.Ticket.TrainNumber, TrainNumber),
                new KeyValuePair<string, string>(Defines.Ticket.Email, Email),
                new KeyValuePair<string, string>(Defines.Ticket.FullName, FullName),
                new KeyValuePair<string, string>(Defines.Ticket.From, From),
                new KeyValuePair<string, string>(Defines.Ticket.To, To),
                new KeyValuePair<string, string>(Defines.Ticket.SendRequest, Defines.Ticket.SendRequestValue),
                new KeyValuePair<string, string>(Defines.Ticket.TypeOfCarriage, SelectedTypeOfPlace.Split(' ')[0])
            };

            values.AddRange(AdditionalTicketParameters.Where(x => x.IsSelected).Select(additionalTicketParameter =>
            new KeyValuePair<string, string>(Defines.Ticket.Additional, additionalTicketParameter.Parameter)));

            var responseString = await _httpService.Post(_appSettings.Tickets.First(x => x.Name == SelectedCity).Url, values,
                new Windows1251());

            var code = GetCode(responseString);
            if (code == null)
            {
                await _userInteraction.AlertAsync(Defines.Ticket.ErrorRequest);
                return;
            }

            SavePersonalInformation(code);
            await _userInteraction.AlertAsync(string.Format(Defines.Ticket.SuccsessfullyRequest, code));
        }
        private async Task<bool> CheckInput()
        {
            var differenceDays = DepartureDate.DayOfYear - DateTime.Now.DayOfYear;
            if (differenceDays < 3)
            {
                if (differenceDays >= 0 || Math.Abs(differenceDays) > 362)
                {
                    await _userInteraction.AlertAsync(Defines.Ticket.DateError);
                    return false;
                }
            }

            if (FullName == null || FullName.Split(' ').Count() < 3)
            {
                await _userInteraction.AlertAsync(Defines.Ticket.FullNameError);
                return false;
            }
            if (Email == null || !Regex.Match(Email, Defines.Ticket.EmailPattern, RegexOptions.IgnoreCase).Success)
            {
                await _userInteraction.AlertAsync(Defines.Ticket.EmailError);
                return false;
            }

            if (PhoneNumber == null || PhoneNumber.Length < 5)
            {
                await _userInteraction.AlertAsync(Defines.Ticket.PhoneNumberError);
                return false;
            }

            return true;
        }

        private void SavePersonalInformation(string code)
        {
            _appSettings.PersonalInformation = new PersonalInformation
            {
                FullName = FullName,
                City = SelectedCity,
                Email = Email,
                PhoneNumber = PhoneNumber,
                LastCode = code
            };

            _serializableService.Serialize(_appSettings, Defines.Restoring.AppSettings);
        }

        private string GetCode(string html)
        {
            return new Regex(Defines.Ticket.CodePattern, RegexOptions.IgnoreCase).Matches(html).Cast<Match>().Select(m => m.Groups[1].Value).FirstOrDefault();
        }

        private void AdditionalParameterSelected(AdditionalTicketParameter x)
        {
            x.IsSelected = !x.IsSelected;
        }
    }
}