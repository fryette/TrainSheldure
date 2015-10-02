using System.Collections.Generic;
using Newtonsoft.Json;
using Trains.Infrastructure.Interfaces;

namespace Trains.Services.Services
{
	class SerializeService : ISerializableService
	{
		public T Desserialize<T>(string json) where T : class
		{
			return json == null ? null : JsonConvert.DeserializeObject<T>(json);
		}
	}
}
