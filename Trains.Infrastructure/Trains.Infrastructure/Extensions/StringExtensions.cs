using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Trains.Infrastructure.Extensions
{
	public static class StringExtensions
	{
		public static IEnumerable<Match> ParseAsHtml( this string data, string pattern)
		{
			return new Regex(pattern, RegexOptions.Singleline).Matches(data).Cast<Match>();
		}
	}
}