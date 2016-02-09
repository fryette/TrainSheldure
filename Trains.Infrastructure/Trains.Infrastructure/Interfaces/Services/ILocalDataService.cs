using System.Threading.Tasks;

namespace Trains.Infrastructure.Interfaces.Services
{
	public interface ILocalDataService
	{
		Task<T> GetLanguageData<T>(string jsonText) where T : class;
		Task<T> GetOtherData<T>(string jsonText) where T : class;
	}
}
