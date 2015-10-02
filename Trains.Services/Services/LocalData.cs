using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Trains.Infrastructure;
using Trains.Infrastructure.Interfaces;

namespace Trains.Services.Services
{
	public class LocalData : ILocalDataService
	{
		readonly IAppSettings _appSettings;
		public LocalData(IAppSettings appSettings)
		{
			_appSettings = appSettings;
		}

		public async Task<T> GetLanguageData<T>(string jsonText) where T : class
		{
			var text = (await new BaseHttpService().LoadResponseAsync(new Uri(Defines.Uri.LanguagesUri + _appSettings.Language.Id + '/' + jsonText + "?badHeader=" + new Random().Next(0, 1000))));
			return text == null ? null : JsonConvert.DeserializeObject<T>(text);
		}

		public async Task<T> GetOtherData<T>(string jsonText) where T : class
		{
			var text = (await new BaseHttpService().LoadResponseAsync(new Uri(Defines.Uri.PatternsUri + '/' + jsonText + "?badHeader=" + new Random().Next(0, 1000))));
			return text == null ? null : JsonConvert.DeserializeObject<T>(text);
		}
	}
}