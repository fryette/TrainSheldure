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
            var text = (await new BaseHttpService().LoadResponseAsync(new Uri(Constants.LanguagesUri + _appSettings.Language.Id + '/' + jsonText)));
            return JsonConvert.DeserializeObject<T>(text);
        }

        public async Task<T> GetOtherData<T>(string jsonText)
        {
            var text = (await new BaseHttpService().LoadResponseAsync(new Uri(Constants.PatternsUri + '/' + jsonText)));
            return JsonConvert.DeserializeObject<T>(text);
        }
    }
}