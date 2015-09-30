using System;
using System.Collections.Generic;
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
			//Resource = Mvx.Resolve<ISerializableService>().Desserialize<Dictionary<string, string>>(Defines.Restoring.ResourceLoader);
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
		}
	}
}
