using System;
using System.Collections.Generic;
using System.Linq;
using TrainShedule_HubVersion.DataModel;

namespace TrainShedule_HubVersion.Infrastructure
{
    class TrainPointsGrabber
    {
        private const string Url = "http://rw.by/";
        const string Pattern = @"arrStations.push\(\""(.+?)""\)";

        public static IEnumerable<string> GetTrainPoints()
        {
            var match = Parser.GetData(Url, Pattern).ToList();
            return !match.Any() ? null : CorrectingCityes(match.Select(point => point.Groups[1].Value));
        }

        private static IEnumerable<string> CorrectingCityes(IEnumerable<string> points)
        {
            if (points == null) return null;
            var list = new List<string>();
            foreach (var point in points)
            {
                if (point == "Картузская") list.Add("Берёза-Картузская");
                if (point == "Минск (Институт Культуры)") list.Add("Институт Культуры");
                var index = point.IndexOf("(", StringComparison.Ordinal);
                list.Add(index == -1 ? point : point.Remove(index));
            }
            return list;
        }
    }
}