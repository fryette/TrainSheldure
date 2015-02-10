using System.Collections.Generic;
using System.Linq;


namespace TrainShedule_HubVersion.DataModel
{
    class TrainPointsGrabber
    {
        private const string Url = "http://rw.by/";
        const string Pattern = @"arrStations.push\(\""(.+?)""\)";
       
        public static IEnumerable<string> GetTrainPoints()
        {
            var match = Parser.GetData(Url, Pattern).ToList();
            return !match.Any() ? null : match.Select(point => point.Groups[1].Value);    
        }
    }
}
