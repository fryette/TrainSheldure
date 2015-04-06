using System.Threading.Tasks;
using Trains.Infrastructure.Infrastructure;
//using Windows.ApplicationModel.Resources;
using Trains.Model.Entities;
using Trains.Services.Interfaces;
using Trains.Services.Tools;
using System.Linq;
using Trains.Infrastructure.Interfaces;
using System.Collections.Generic;
using Trains.Entities;
using Trains.Resources;
using System;
using System.Reflection;
using System.Resources;

namespace Trains.Services.Implementations
{
    public class Start : IStartService
    {
        private readonly ISerializableService _serializable;
        private readonly ILocalDataService _local;
        private readonly IAppSettings _appSettings;

        public Start(ISerializableService serializable, ILocalDataService local, IAppSettings appSettings)
        {
            _serializable = serializable;
            _local = local;
            _appSettings = appSettings;
        }

        public async Task RestoreData()
        {
            if (_appSettings.AutoCompletion != null) return;
            //SavedItems.ResourceLoader = ResourceLoader.GetForViewIndependentUse("Resources");
            CheckIsFirstStart();
            await Task.Run(() => StartedActions());
            await Task.Delay(2000);
        }

        private async void StartedActions()
        {
            var assembly = typeof(Constants).GetTypeInfo().Assembly;
            //TODO выбор языка не стоит,захардокадано первый resource манифест 
            _appSettings.Resource = new ResourceManager(assembly.GetManifestResourceNames()[0].Replace(".resources", String.Empty), assembly);
            _appSettings.AutoCompletion = (await _local.GetStopPoints()).SelectMany(dataGroup => dataGroup.Items);
            _appSettings.HelpInformation = (await _local.GetHelpInformations()).SelectMany(dataGroup => dataGroup.Items);
            _appSettings.FavoriteRequests = await _serializable.ReadObjectFromXmlFileAsync<List<LastRequest>>(Constants.FavoriteRequests);
            _appSettings.UpdatedLastRequest = await _serializable.ReadObjectFromXmlFileAsync<LastRequest>(Constants.UpdateLastRequest);
            _appSettings.LastRequestTrain = await _serializable.ReadObjectFromXmlFileAsync<List<Train>>(Constants.LastTrainList);
        }

        private async void CheckIsFirstStart()
        {
            if ((await _serializable.CheckIsFile(Constants.IsFirstStart)))
                _appSettings.LastRequests = await _serializable.ReadObjectFromXmlFileAsync<List<LastRequest>>(Constants.LastRequests);
            else
            {
                //ToolHelper.ShowMessageBox(SavedItems.ResourceLoader.GetString("FirstMessageStartString"));
                await Task.Run(() => _serializable.SerializeObjectToXml(true, Constants.IsFirstStart));
                await _serializable.DeleteFile(Constants.IsSecondStart);
                await _serializable.DeleteFile(Constants.LastRequests);
            }
        }

    }
}
