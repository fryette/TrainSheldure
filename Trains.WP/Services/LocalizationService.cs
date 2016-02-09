using System;
using Windows.ApplicationModel.Resources;
using Trains.Infrastructure.Interfaces;

namespace Trains.WP.Services
{
	public class LocalizationService : ILocalizationService
	{
		private readonly ResourceLoader _resourceLoader;

		public LocalizationService()
		{
			_resourceLoader = ResourceLoader.GetForCurrentView();
		}

		public string GetString(string key)
		{
			return _resourceLoader.GetString(key);
		}
	}
}
