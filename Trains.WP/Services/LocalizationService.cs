﻿using System;
using System.Globalization;
using Windows.ApplicationModel.Resources;
using Windows.ApplicationModel.Resources.Core;
using Windows.Globalization;
using Trains.Infrastructure;
using Trains.Infrastructure.Interfaces;

namespace Trains.WP.Services
{
	public class LocalizationService : ILocalizationService
	{
		private readonly ResourceLoader _resourceLoader;
		private readonly ISerializableService _serializable;

		public LocalizationService(ISerializableService serializable)
		{
			_serializable = serializable;
			_resourceLoader = ResourceLoader.GetForCurrentView();
		}

		public string CurrentLanguageId => _serializable.Desserialize<Model.Entities.Language>(Defines.Restoring.AppLanguage)?.Id;

		public string GetString(string key)
		{
			return _resourceLoader.GetString(key);
		}

		public void ChangeLocale(string newLocale)
		{
			ResourceContext.ResetGlobalQualifierValues();

			var newCulture = new CultureInfo(newLocale);

			CultureInfo.DefaultThreadCurrentUICulture = newCulture;
			CultureInfo.DefaultThreadCurrentCulture = newCulture;

			ApplicationLanguages.PrimaryLanguageOverride = newLocale;
		}
	}
}
