using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;


namespace TrainShedule_HubVersion.DataModel
{
    class TrainPointsGrabber
    {
        private const string Url = "http://rw.by/";
        const string Pattern = @"arrStations.push\(\""(.+?)""\)";
       
        public static IEnumerable<string> GetTrainsPoints()
        {
            return GetTrainPoints(Parser.GetData(Url,Pattern));
        }
        private static IEnumerable<string> GetTrainPoints(IEnumerable<Match> match)
        {
            var enumerable = match as IList<Match> ?? match.ToList();
            return !enumerable.Any() ? null : enumerable.Select(point => point.Groups[1].Value);
        }
    }
}
