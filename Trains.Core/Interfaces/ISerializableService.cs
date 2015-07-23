namespace Trains.Core.Interfaces
{
	public interface ISerializableService
	{
		bool Exists(string fileName);
		void Serialize<T>(T obj, string fileName);
		void Delete(string fileName);
		T Desserialize<T>(string filename) where T : class;
		void ClearAll();
	}
}
