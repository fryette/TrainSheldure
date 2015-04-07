using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Threading.Tasks;
using Trains.Model.Entities;
using Trains.Services.Interfaces;

namespace Trains.Service.Implementation
{
    public class LocalData : ILocalDataService
    {
		public async Task<string> LoadContent(string fileName)
		{
			var assembly = typeof(Trains.Resources.Constants).GetTypeInfo().Assembly;
			var name = string.Format("Trains.Resources.DataModels.{0}", fileName);
			var stream = assembly.GetManifestResourceStream(name);
			
			if (stream != null) {
				using (var reader = new StreamReader(stream))
				{
					return await reader.ReadToEndAsync();
				}
			}
			return null;
		}

        public async Task<List<CountryStopPointGroup>> GetStopPoints()
        {
			var json = await LoadContent("StopPointsru.json");
            return JsonDeserializer<List<CountryStopPointGroup>>(json);
        }

        public async Task<List<HelpInformationGroup>> GetHelpInformations()
        {
			var json = await LoadContent("HelpInformationRU.json");
            return JsonDeserializer<List<HelpInformationGroup>>(json);
        }

        private T JsonDeserializer<T>(string jsonText) where T : class
        {
            return JsonConvert.DeserializeObject<T>(jsonText);
        }
    }
}