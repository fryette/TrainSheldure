﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;

namespace Trains.WP.Infrastructure
{
    class Parser
    {
        public static IEnumerable<Match> GetData(string url, string pattern)
        {
            return ParseTrainData(GetHtmlCode(url), pattern);
        }

        public static string GetHtmlCode(string url)
        {
            var httpClient = new HttpClient();
            var httpResponseMessage = httpClient.GetAsync(url + '&' + new Random().Next(0, 9)).Result;
            var res = httpResponseMessage.Content.ReadAsStreamAsync().Result;
            return new StreamReader(res, Encoding.UTF8).ReadToEnd();
        }

        public static IEnumerable<Match> ParseTrainData(string data, string pattern)
        {
            return new Regex(pattern, RegexOptions.Singleline).Matches(data).Cast<Match>();
        }
    }
}