using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.ApplicationModel.Resources.Core;
using Trains.Model.Entities;
using Trains.Services.Interfaces;
using Trains.Services.Tools;
using Language = Trains.Model.Entities.Language;

namespace Trains.Services.Implementations
{
    public class Start : IStartService
    {
        private const string FavoriteString = "favoriteRequests";
        private const string LastRequestString = "lastRequests";

        private const string UpdateLastRequestString = "updateLastRequst";
        private const string IsSecondStartString = "isSecondStart";
        private const string IsFirstStartString = "isFirstStart";


        private readonly ISerializableService _serializable;
        private readonly ISearchService _search;

        public Start(ISerializableService serializable, ISearchService search)
        {
            _serializable = serializable;
            _search = search;
        }

        public async Task RestoreData()
        {
            if (SavedItems.AutoCompletion != null) return;
            SavedItems.Lang = (await _serializable.ReadObjectFromXmlFileAsync<Language>("currentLanguage"));
            var context = ResourceContext.GetForCurrentView();
            context.Languages = new List<string> { SavedItems.Lang == null ? "ru" : SavedItems.Lang.Id};
            SavedItems.ResourceLoader = ResourceLoader.GetForViewIndependentUse("Resources");
            CheckIsFirstStart();
            await Task.Run(() => StartedActions());
            await Task.Delay(2000);
        }

        private async void StartedActions()
        {
            SavedItems.AutoCompletion = await Task.Run(() => _search.GetCountryStopPoint());
             SavedItems.FavoriteRequests = await Task.Run(() => _serializable.GetLastRequests(FavoriteString));
            SavedItems.UpdatedLastRequest = await Task.Run(() => _serializable.ReadObjectFromXmlFileAsync<LastRequest>(UpdateLastRequestString));
            SavedItems.UpdatedLastRequest = null;
        }

        private async void CheckIsFirstStart()
        {
            if ((await _serializable.CheckIsFile(IsFirstStartString)))
            {
                SavedItems.LastRequests = await Task.Run(() => _serializable.GetLastRequests(LastRequestString));
            }
            else
            {
                ToolHelper.ShowMessageBox(SavedItems.ResourceLoader.GetString("FirstMessageStartString"));
                await Task.Run(() => _serializable.SerializeObjectToXml(true, IsFirstStartString));
                _serializable.DeleteFile(IsSecondStartString);
                _serializable.DeleteFile(LastRequestString);
            }
        }
    }
}
