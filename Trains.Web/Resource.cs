using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Trains.Web
{
	public static class Resource
	{
		private static Dictionary<string, string> resourceDictionary;
		static Resource()
		{
			var line = "";
			using (var sr = new StreamReader(@"D:\Git\TrainsMobile\Trains.Web\Resources\ru\Resource.json"))
			{
				// Read the stream to a string, and write the string to the console.
				line = sr.ReadToEnd();
			}
			resourceDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(line);
		}
		public static string GetResource(string key)
		{
			if (string.IsNullOrEmpty(key))
				throw new ArgumentException("Bad key in resource");
			return resourceDictionary[key];
		}
	}
}