using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using Cirrious.MvvmCross.Platform;
using Cirrious.MvvmCross.ViewModels;
using Newtonsoft.Json;
using Trains.Core.Extensions;
using Trains.Core.Interfaces;
using Trains.Core.Services.Interfaces;
using Trains.Entities;
using FormUrlEncodedContent = Trains.Core.Extensions.FormUrlEncodedContent;

namespace Trains.Core.ViewModels
{
    public class BookingViewModel : MvxViewModel
    {
        #region readonlyProperties

        private readonly IAppSettings _appSettings;
        private readonly IHttpService _httpService;

        #endregion

        #region ctor

        public BookingViewModel(IAppSettings appSettings, IHttpService httpService)
        {
            _appSettings = appSettings;
            _httpService = httpService;
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

        public void Init(string param)
        {
            var train = JsonConvert.DeserializeObject<Train>(param);
            From = _appSettings.UpdatedLastRequest.Route.From;
            To = _appSettings.UpdatedLastRequest.Route.To;
            TrainNumber = train.City.Split(' ')[0].Substring(0, 3);
            DepartureTime = train.StartTime;
            SendRequest();
        }

        public async void SendRequest()
        {
            var values = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("textValue(kodd)",""),
                new KeyValuePair<string, string>("textValue(poezd_ii)",""),
                new KeyValuePair<string, string>("textValue(tip_vag_ii)","П"),
                new KeyValuePair<string, string>("textValue(kol_m_ii)",""),
                new KeyValuePair<string, string>("textValue(dat_o_ii)",""),
                new KeyValuePair<string, string>("textValue(tel)","+3752920042"),
                new KeyValuePair<string, string>("textValue(hid)","-1"),
                new KeyValuePair<string, string>("textValue(dostavka)","0"),
                new KeyValuePair<string, string>("textValue(kol_mest)","1"),
                new KeyValuePair<string, string>("textValue(dat_o)","21.08.2015"),
                new KeyValuePair<string, string>("textValue(poezd)",TrainNumber),
                new KeyValuePair<string, string>("textValue(email)","2004195@gmail.com"),
                new KeyValuePair<string, string>("textValue(f_zakaz)","Иван Кореливич Игорев"),
                new KeyValuePair<string, string>("textValue(nsto)",From),
                new KeyValuePair<string, string>("textValue(nstn)",To),
                new KeyValuePair<string, string>("send_z1","Отправить заявку"),
                new KeyValuePair<string, string>("textValue(tip_vag)","П")
            };

            //var content = new FormUrlEncodedContent(values);
            //var httpClient = new HttpClient(new HttpClientHandler());
            //var response = await httpClient.PostAsync(new Uri("http://www.brestrw.by/zpdbrnv/zayavka.do"), content);
            //var temp = await response.Content.ReadAsByteArrayAsync();
            //var responseString = new Windows1251().GetString(temp, 0, temp.Length - 1);
            var responseString = await _httpService.Post(new Uri("http://www.brestrw.by/zpdbrnv/zayavka.do"), values,
                new Windows1251());
        }
    }

}
