namespace Trains.Infrastructure.Interfaces
{
	public interface IJsonConverter
	{
		T Deserialize<T>(string jsonString);
		string Serialize(object obj);
	}
}
