using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;
using System;
using System.Globalization;
using Trains.Core.Interfaces;
using Trains.Core;

namespace Trains.Core.Service
{
    public class LocalData : ILocalDataService
    {
        IAppSettings _appSettings;
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