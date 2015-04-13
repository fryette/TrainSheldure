using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Threading.Tasks;
using Trains.Core.Interfaces;
using Trains.Model.Entities;
using Trains.Services.Interfaces;

namespace Trains.Core
{
    public class LocalData : ILocalDataService
    {
        private async Task<string> LoadContent(string fileName)
        {
            var assembly = typeof(Trains.Resources.Constants).GetTypeInfo().Assembly;
            var name = string.Format("Trains.Resources.DataModels.{0}", fileName);
            var stream = assembly.GetManifestResourceStream(name);

            if (stream != null)
            {
                using (var reader = new StreamReader(stream))
                {
                    return await reader.ReadToEndAsync();
                }
            }
            return null;
        }

        public async Task<T> GetData<T>(string fileName) where T : class
        {
            var jsonText = await LoadContent(fileName);
            return JsonConvert.DeserializeObject<T>(jsonText);
        }
    }
}