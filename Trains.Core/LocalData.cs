using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Trains.Core.Interfaces;
using Trains.Resources;

namespace Trains.Core
{
    public class LocalData : ILocalDataService
    {
        private async Task<string> LoadContent(string fileName)
        {
            var assembly = typeof(Constants).GetTypeInfo().Assembly;
            var name = string.Format("Trains.Resources.DataModels.{0}", fileName);
            var stream = assembly.GetManifestResourceStream(name);

            if (stream == null) return null;
            using (var reader = new StreamReader(stream))
            {
                return await reader.ReadToEndAsync();
            }
        }

        public async Task<T> GetData<T>(string fileName) where T : class
        {
            var jsonText = await LoadContent(fileName);
            return JsonConvert.DeserializeObject<T>(jsonText);
        }
    }
}