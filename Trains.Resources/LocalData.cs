using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;

namespace Trains.Resources
{
    public class LocalData : ILocalDataService
    {
        private async Task<string> LoadContent(string fileName, string lang)
        {
            var assembly = typeof(Constants).GetTypeInfo().Assembly;
            var name = string.Format("Trains.Resources.DataModels.{0}.{1}",lang, fileName);
            var stream = assembly.GetManifestResourceStream(name);

            if (stream == null) return null;
            using (var reader = new StreamReader(stream))
                return await reader.ReadToEndAsync();
        }

        public async Task<T> GetData<T>(string fileName, string lang) where T : class
        {
            var jsonText = await LoadContent(fileName, lang);
            return JsonConvert.DeserializeObject<T>(jsonText);
        }
    }
}