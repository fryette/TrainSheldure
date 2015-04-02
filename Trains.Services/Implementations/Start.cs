using System.Threading.Tasks;
using Trains.Infrastructure.Infrastructure;
//using Windows.ApplicationModel.Resources;
using Trains.Model.Entities;
using Trains.Services.Interfaces;
using Trains.Services.Tools;
using System.Linq;
using Trains.Infrastructure.Interfaces;

namespace Trains.Services.Implementations
{
    public class Start : IStartService
    {
        private readonly ISerializableService _serializable;
        private readonly ILocalDataService _local;

        public Start(ISerializableService serializable, ILocalDataService local)
        {
            _serializable = serializable;
            _local = local;
        }

        public async Task RestoreData()
        {
            if (SavedItems.AutoCompletion != null) return;
            //SavedItems.ResourceLoader = ResourceLoader.GetForViewIndependentUse("Resources");
            CheckIsFirstStart();
            await Task.Run(() => StartedActions());
            await Task.Delay(2000);
        }

        private async void StartedActions()
        {
            SavedItems.AutoCompletion = (await _local.GetData()).SelectMany(dataGroup => dataGroup.Items);

            SavedItems.FavoriteRequests = await Task.Run(() => _serializable.GetLastRequests(FileName.FavoriteRequests));
            SavedItems.UpdatedLastRequest = await Task.Run(() => _serializable.ReadObjectFromXmlFileAsync<LastRequest>(FileName.UpdateLastRequest));
        }

        private async void CheckIsFirstStart()
        {
            if ((await _serializable.CheckIsFile(FileName.IsFirstStart)))
                SavedItems.LastRequests = await Task.Run(() => _serializable.GetLastRequests(FileName.LastRequests));
            else
            {
                //ToolHelper.ShowMessageBox(SavedItems.ResourceLoader.GetString("FirstMessageStartString"));
                await Task.Run(() => _serializable.SerializeObjectToXml(true, FileName.IsFirstStart));
                _serializable.DeleteFile(FileName.IsSecondStart);
                _serializable.DeleteFile(FileName.LastRequests);
            }
        }
    }
}
