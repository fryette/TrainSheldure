using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Trains.Infrastructure.Interfaces;
using Trains.Model.Entities;

namespace Trains.Infrastructure.Infrastructure
{
    public class CountryStopPointData : ILocalDataService
    {
        private readonly IFileSystem _fileSystem;

        public CountryStopPointData(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public async Task<List<CountryStopPointGroup>> GetStopPoints()
        {
            return JsonDeserializer<List<CountryStopPointGroup>>(await _fileSystem.GetFileContents("ms-appx:///trains.model/datamodel/StopPointsRU.json"));
        }

        public async Task<List<HelpInformationGroup>> GetHelpInformations()
        {
            return JsonDeserializer<List<HelpInformationGroup>>(await _fileSystem.GetFileContents("ms-appx:///trains.model/datamodel/HelpInformationRU.json"));
        }

        private T JsonDeserializer<T>(string jsonText) where T : class
        {
            return JsonConvert.DeserializeObject<T>(jsonText);
        }
    }
}