using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using Cirrious.MvvmCross.Platform;
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
                new KeyValuePair<string, string>("send_z","Отправить заявку"),
                new KeyValuePair<string, string>("textValue(tip_vag)","П")
            };

            //string body = "textValue(hid)=-1&textValue(dostavka)=0&textValue(kol_m_ii)=1&textValue(f_zakaz)=12 21 21&" +
            //                    "textValue(dat_o)=21.08.2015&textValue(dat_o_ii)=20.08.2015&textValue(nsto)=21&textValue(nstn)=21&" +
            //                    "textValue(poezd)=212&textValue(tel)=21212121&textValue(poezd_ii)=213&textValue(email)=21@21.com&" +
            //                    "textValue(kol_mest)=3&textValue(kodd)=&o_usl1=Кроме боковых мест&textValue(tip_vag)=Л" +
            //                    "textValue(tip_vag_ii)=Лу&send_z=Отправить заявку";

            var content = new Trains.Core.ViewModels.FormUrlEncodedContent(values);
            var httpClient = new HttpClient(new HttpClientHandler());
            var response = await httpClient.PostAsync(new Uri("http://www.brestrw.by/zpdbrnv/zayavka.do"), content);
            var temp = await response.Content.ReadAsByteArrayAsync();
            var responseString = new Windows1251().GetString(temp, 0, temp.Length - 1);
        }

        public string ConvertToUtf8(string str)
        {
            var srcEncodingFormat = Encoding.UTF8;
            Encoding dstEncodingFormat = new Windows1251();
            var originalByteString = srcEncodingFormat.GetBytes(str);
            var convertedByteString = Encoding.Convert(srcEncodingFormat,
            dstEncodingFormat, originalByteString);
            var finalString = dstEncodingFormat.GetString(convertedByteString, 0, convertedByteString.Length - 1);
            return finalString;
        }
    }

    public class Windows1251 : Encoding
    {
        static string alpha = "\0\u0001\u0002\u0003\u0004\u0005\u0006\a\b\t\n\v\f\r\u000e\u000f\u0010\u0011\u0012\u0013\u0014\u0015\u0016\u0017\u0018\u0019\u001a\u001b\u001c\u001d\u001e\u001f !\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~\u007fЂЃ‚ѓ„…†‡€‰Љ‹ЊЌЋЏђ‘’“”•–—\u0098™љ›њќћџ ЎўЈ¤Ґ¦§Ё©Є«¬­®Ї°±Ііґµ¶·ё№є»јЅѕїАБВГДЕЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯабвгдежзийклмнопрстуфхцчшщъыьэюя";
        public override int GetByteCount(char[] chars, int index, int count)
        {
            return count;
        }

        public override string GetString(byte[] bytes, int index, int count)
        {
            var newBytes = new byte[bytes.Length+1];
            for (var i = 0; i < bytes.Length; i++)
            {
                newBytes[i] = bytes[i];
            }
            newBytes[bytes.Length] = 0;
            return base.GetString(newBytes, index, count+1);
        }

        public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
        {
            var questionIndex = (byte)alpha.IndexOf('?');
            for (var i = 0; i < charCount; i++)
            {
                var toIndex = byteIndex + i;
                var index = alpha.IndexOf(chars[charIndex + i]);
                if (index == -1)
                    bytes[toIndex] = questionIndex;
                else
                    bytes[toIndex] = (byte)index;
            }
            return charCount;
        }

        public override int GetCharCount(byte[] bytes, int index, int count)
        {
            return count;
        }

        public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
        {
            for (var i = 0; i < bytes.Length - 1; i++)
            {
                chars[i + charIndex] = alpha[bytes[byteIndex + i]];
            }
            return byteCount;
        }

        public override int GetMaxByteCount(int charCount)
        {
            return charCount;
        }

        public override int GetMaxCharCount(int byteCount)
        {
            return byteCount;
        }
    }

    public class FormUrlEncodedContent : ByteArrayContent
    {
        public FormUrlEncodedContent(IEnumerable<KeyValuePair<string, string>> nameValueCollection)
            : base(EncodeContent(nameValueCollection))
        {
            Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
        }

        static byte[] EncodeContent(IEnumerable<KeyValuePair<string, string>> nameValueCollection)
        {
            if (nameValueCollection == null)
                throw new ArgumentNullException("nameValueCollection");

            //
            // Serialization as application/x-www-form-urlencoded
            //
            // Element nodes selected for inclusion are encoded as EltName=value{sep}, where = is a literal
            // character, {sep} is the separator character from the separator attribute on submission,
            // EltName represents the element local name, and value represents the contents of the text node.
            //
            // The encoding of EltName and value are as follows: space characters are replaced by +, and then
            // non-ASCII and reserved characters (as defined by [RFC 2396] as amended by subsequent documents
            // in the IETF track) are escaped by replacing the character with one or more octets of the UTF-8
            // representation of the character, with each octet in turn replaced by %HH, where HH represents
            // the uppercase hexadecimal notation for the octet value and % is a literal character. Line breaks
            // are represented as "CR LF" pairs (i.e., %0D%0A).
            //
            var sb = new List<byte>();
            foreach (var item in nameValueCollection)
            {
                if (sb.Count != 0)
                    sb.Add((byte)'&');

                var data = SerializeValue(item.Key);
                if (data != null)
                    sb.AddRange(data);
                sb.Add((byte)'=');

                data = SerializeValue(item.Value);
                if (data != null)
                    sb.AddRange(data);
            }

            return sb.ToArray();
        }

        static byte[] SerializeValue(string value)
        {
            if (value == null)
                return null;
            value = ConvertToUtf8(value);
            return new Windows1251().GetBytes(value);
        }

        public static string ConvertToUtf8(string str)
        {
            if (str == "") return str;
            var srcEncodingFormat = Encoding.UTF8;
            Encoding dstEncodingFormat = new Windows1251();
            var originalByteString = srcEncodingFormat.GetBytes(str);
            var convertedByteString = Encoding.Convert(srcEncodingFormat,
            dstEncodingFormat, originalByteString);
            var finalString = dstEncodingFormat.GetString(convertedByteString, 0, convertedByteString.Length - 1);
            return finalString;
        }
    }
}
