using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Trains.Infrastructure.Interfaces;

namespace Trains.Infrastructure
{
	public sealed class ResourceLoader
	{
		private static volatile ResourceLoader _instance;
		private static readonly object SyncRoot = new Object();

		public Dictionary<string, string> Resource;

		private ResourceLoader()
		{
			using (var sr = new StreamReader(@"D:\Git\TrainsMobile\Trains.Web\Resources\ru\Resource.json"))
			{
				// Read the stream to a string, and write the string to the console.
				Resource = JsonConvert.DeserializeObject<Dictionary<string, string>>(sr.ReadToEnd());
			}
		}

		public static ResourceLoader Instance
		{
			get
			{
				if (_instance != null) return _instance;
				lock (SyncRoot)
				{
					if (_instance == null)
					{
						_instance = new ResourceLoader();
					}
				}
				return _instance;
			}

			set { _instance = value; }
		}

		
	}
}
