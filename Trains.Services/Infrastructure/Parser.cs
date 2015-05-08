using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Trains.Services.Infrastructure
{
   public class Parser
    {
        public static IEnumerable<Match> ParseData(string data, string pattern)
        {
            return new Regex(pattern, RegexOptions.Singleline).Matches(data).Cast<Match>();
        }
    }
}