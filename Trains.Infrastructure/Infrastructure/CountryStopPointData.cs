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

        public async Task<List<CountryStopPointDataGroup>> GetData()
        {
            var datauri = new Uri("ms-appx:///trains.model/datamodel/stoppointsru.json");
            var jsontext = await _fileSystem.GetFileContents("ms-appx:///trains.model/datamodel/stoppointsru.json");
            return JsonConvert.DeserializeObject<List<CountryStopPointDataGroup>>(jsontext);
        }
    }
}
