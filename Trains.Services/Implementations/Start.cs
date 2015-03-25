using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Globalization;
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

        public async void RestoreData()
        {
            if (SavedItems.AutoCompletion != null) return;
            var lang = (await _serializable.ReadObjectFromXmlFileAsync<Language>("currentLanguage"));
            ApplicationLanguages.PrimaryLanguageOverride = lang == null ? "ru" : lang.Id;
            SavedItems.ResourceLoader = ResourceLoader.GetForViewIndependentUse("Resources");
            CheckIsFirstStart();
            await Task.Run(() => StartedActions());
            await Task.Delay(2000);
        }

        private async void StartedActions()
        {
            SavedItems.AutoCompletion = await Task.Run(() => _search.GetCountryStopPoint());
            try
            {
                SavedItems.UpdatedLastRequest = await Task.Run(() => _serializable.ReadObjectFromXmlFileAsync<LastRequest>(UpdateLastRequestString));
            }
            catch (Exception)
            {
                SavedItems.UpdatedLastRequest = null;
            }
        }

        private async void CheckIsFirstStart()
        {
            if ((await _serializable.CheckIsFile(IsSecondStartString)))
                await Task.Run(() => SerializationData());
            else
            {
                ToolHelper.ShowMessageBox(SavedItems.ResourceLoader.GetString("FirstMessageStartString"));
                await Task.Run(() => _serializable.SerializeObjectToXml(true, IsSecondStartString));
                if (await _serializable.CheckIsFile(IsFirstStartString))
                    _serializable.DeleteFile(IsFirstStartString);
                if (await _serializable.CheckIsFile(LastRequestString))
                    _serializable.DeleteFile(LastRequestString);
            }
        }

        private async void SerializationData()
        {
            SavedItems.LastRequests = await Task.Run(() => _serializable.GetLastRequests(LastRequestString));
            SavedItems.FavoriteRequests = await Task.Run(() => _serializable.GetLastRequests(FavoriteString));
        }
    }
}
