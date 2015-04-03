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
            _appSettings.AutoCompletion = (await _local.GetStopPoints()).SelectMany(dataGroup => dataGroup.Items);
            _appSettings.HelpInformation = (await _local.GetHelpInformations()).SelectMany(dataGroup => dataGroup.Items);
            _appSettings.FavoriteRequests = await _serializable.ReadObjectFromXmlFileAsync<List<LastRequest>>(FileName.FavoriteRequests);
            _appSettings.UpdatedLastRequest = await _serializable.ReadObjectFromXmlFileAsync<LastRequest>(FileName.UpdateLastRequest);
            _appSettings.LastRequestTrain = await _serializable.ReadObjectFromXmlFileAsync<List<Train>>(FileName.LastTrainList);
        }

        private async void CheckIsFirstStart()
        {
            if ((await _serializable.CheckIsFile(FileName.IsFirstStart)))
                _appSettings.LastRequests = await _serializable.ReadObjectFromXmlFileAsync<List<LastRequest>>(FileName.LastRequests);
            else
            {
                //ToolHelper.ShowMessageBox(SavedItems.ResourceLoader.GetString("FirstMessageStartString"));
                await Task.Run(() => _serializable.SerializeObjectToXml(true, FileName.IsFirstStart));
                await _serializable.DeleteFile(FileName.IsSecondStart);
                await _serializable.DeleteFile(FileName.LastRequests);
            }
        }

    }
}
