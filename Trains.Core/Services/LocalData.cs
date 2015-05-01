using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Trains.Core.Interfaces;

namespace Trains.Core.Services
{
    public class LocalData : ILocalDataService
    {
        readonly IAppSettings _appSettings;
        public LocalData(IAppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public async Task<T> GetData<T>(string jsonText) where T : class
        {
            var text = (await new BaseHttpService().LoadResponseAsync(new Uri(Constants.LanguagesUri + _appSettings.Language.Id + '/' + jsonText)));
            return JsonConvert.DeserializeObject<T>(text);
        }
    }
}