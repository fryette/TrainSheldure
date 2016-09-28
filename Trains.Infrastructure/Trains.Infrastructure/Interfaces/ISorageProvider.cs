namespace Trains.Infrastructure.Interfaces
{
	public interface ISorageProvider
	{
		bool Exists(string fileName);
		void Save<T>(T obj, string fileName);
		void TryToRemove(string fileName);
		T ReadAndMap<T>(string filename) where T : class;
		void ClearAll();
	}
}
