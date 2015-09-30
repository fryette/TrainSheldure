namespace Trains.Infrastructure.Interfaces
{
	public interface ISerializableService
	{
		T Desserialize<T>(string filename) where T : class;
	}
}
