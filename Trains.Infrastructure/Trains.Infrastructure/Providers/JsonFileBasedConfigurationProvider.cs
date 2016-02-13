using System.IO;
using System.Reflection;
using Trains.Infrastructure.Interfaces;
using Trains.Infrastructure.Interfaces.Providers;

namespace Trains.Infrastructure.Providers
{
	public class JsonFileBasedConfigurationProvider : IConfigurationProvider
	{
		private readonly IJsonConverter _jsonConverter;

		private const string SettingsPathFormat = "Trains.Infrastructure.Settings.{0}.json";

		public JsonFileBasedConfigurationProvider(IJsonConverter jsonConverter)
		{
			_jsonConverter = jsonConverter;
		}
		public T Load<T>(string configName) where T : class
		{
			var assembly = GetType().GetTypeInfo().Assembly;
			var stream = assembly.GetManifestResourceStream(string.Format(SettingsPathFormat, configName));

			using (var reader = new StreamReader(stream))
			{
				return _jsonConverter.Deserialize<T>(reader.ReadToEnd());
			}
		}
	}
}
