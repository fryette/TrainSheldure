using System.Collections.Generic;
using System.IO;
using System.Web;
using Newtonsoft.Json;
using Trains.Models;

namespace Trains.Infrastructure
{
	public static class Countries
	{
		public static List<CountryStopPointItem> CountriesList { get; set; }

		static Countries()
		{
			using (var sr = new StreamReader(HttpContext.Current.Server.MapPath("/Resources/ru/Countries/Belarus.json")))
			{
				CountriesList = JsonConvert.DeserializeObject<List<CountryStopPointItem>>(sr.ReadToEnd());
			}
		}
	}
}
