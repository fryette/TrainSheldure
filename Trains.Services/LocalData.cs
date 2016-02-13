using System;
using System.Threading.Tasks;
using Trains.Infrastructure;
using Trains.Infrastructure.Interfaces;
using Trains.Infrastructure.Interfaces.Platform;
using Trains.Infrastructure.Interfaces.Services;

namespace Trains.Services
{
	public class LocalData : ILocalDataService
	{
		private readonly ILocalizationService _localizationService;
		private readonly IJsonConverter _jsonConverter;

		public LocalData(ILocalizationService localizationService, IJsonConverter jsonConverter)
		{
			_localizationService = localizationService;
			_jsonConverter = jsonConverter;
		}

		public async Task<T> GetLanguageData<T>(string jsonText) where T : class
		{
			var text = await new BaseHttpService().LoadResponseAsync(new Uri(Defines.Uri.LanguagesUri + _localizationService.CurrentLanguageId + '/' + jsonText + "?badHeader=" + new Random().Next(0, 1000)));
			return text == null ? null : _jsonConverter.Deserialize<T>(text);
		}

		public async Task<T> GetOtherData<T>(string jsonText) where T : class
		{
			var text = await new BaseHttpService().LoadResponseAsync(new Uri(Defines.Uri.PatternsUri + '/' + jsonText + "?badHeader=" + new Random().Next(0, 1000)));
			return text == null ? null : _jsonConverter.Deserialize<T>(text);
		}
	}
}
