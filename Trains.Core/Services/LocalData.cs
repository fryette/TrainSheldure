using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Trains.Core.Interfaces;
using Trains.Core.Resources;
using Trains.Core.Services.Interfaces;

namespace Trains.Core.Services
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
			return JsonConvert.DeserializeObject<T>(text);
		}

		public async Task<T> GetOtherData<T>(string jsonText)
		{
			var text = (await new BaseHttpService().LoadResponseAsync(new Uri(Defines.Uri.PatternsUri + '/' + jsonText + "?badHeader=" + new Random().Next(0, 1000))));
			return JsonConvert.DeserializeObject<T>(text);
		}
	}
}