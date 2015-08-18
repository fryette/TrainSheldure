using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using Cirrious.MvvmCross.ViewModels;
using Newtonsoft.Json;
using Trains.Core.Interfaces;
using Trains.Core.Services.Interfaces;
using Trains.Entities;

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
            TrainNumber = train.City.Split(' ')[0];
            DepartureTime = train.StartTime;
            SendRequest();
        }

        public async void SendRequest()
        {
            var values = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("textValue(kodd)",""),
                new KeyValuePair<string, string>("textValue(poezd_ii)",""),
                new KeyValuePair<string, string>("textValue(tip_vag_ii)",""),
                new KeyValuePair<string, string>("textValue(kol_m_ii)",""),
                new KeyValuePair<string, string>("textValue(dat_o_ii)",""),
                new KeyValuePair<string, string>("textValue(tel)","+3752920042"),
                new KeyValuePair<string, string>("textValue(hid)","-1"),
                new KeyValuePair<string, string>("textValue(dostavka)","0"),
                new KeyValuePair<string, string>("textValue(kol_mest)","1"),
                new KeyValuePair<string, string>("textValue(dat_o)","21.08.2015"),
                new KeyValuePair<string, string>("textValue(poezd)","212"),
                new KeyValuePair<string, string>("textValue(email)","21@21.com"),
                new KeyValuePair<string, string>("textValue(f_zakaz)","Ivan Fedorovich Chikaev"),
                new KeyValuePair<string, string>("textValue(nsto)","Брест"),
                new KeyValuePair<string, string>("textValue(nstn)","Минск"),
                new KeyValuePair<string, string>("send_z","Отправить заявку"),
                new KeyValuePair<string, string>("textValue(tip_vag)","П")
            };
            //string body = "textValue(hid)=-1&textValue(dostavka)=0&textValue(kol_m_ii)=1&textValue(f_zakaz)=12 21 21&" +
            //                    "textValue(dat_o)=21.08.2015&textValue(dat_o_ii)=20.08.2015&textValue(nsto)=21&textValue(nstn)=21&" +
            //                    "textValue(poezd)=212&textValue(tel)=21212121&textValue(poezd_ii)=213&textValue(email)=21@21.com&" +
            //                    "textValue(kol_mest)=3&textValue(kodd)=&o_usl1=Кроме боковых мест&textValue(tip_vag)=Л" +
            //                    "textValue(tip_vag_ii)=Лу&send_z=Отправить заявку";
            var content = new FormUrlEncodedContent(values);
            var httpClient = new HttpClient(new HttpClientHandler());
            HttpResponseMessage response = await httpClient.PostAsync(new Uri("http://www.brestrw.by/zpdbrest/zayavka.do"), content);
            var temp = await response.Content.ReadAsByteArrayAsync();
            var responseString = Encoding.UTF8.GetString(temp, 0, temp.Length - 1);
        }

        public string ConvertToUtf8(string str)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            return Encoding.UTF8.GetString(bytes, 0, bytes.Length - 1);
        }
    }
}
