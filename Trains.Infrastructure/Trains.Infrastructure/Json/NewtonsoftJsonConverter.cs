using Newtonsoft.Json;
using Trains.Infrastructure.Interfaces;

namespace Trains.Infrastructure.Json
{
	public class NewtonsoftJsonConverter : IJsonConverter
	{
		public T Deserialize<T>(string jsonString)
		{
			return JsonConvert.DeserializeObject<T>(jsonString);
		}

		public string Serialize(object obj)
		{
			return JsonConvert.SerializeObject(obj);
		}
	}
}
