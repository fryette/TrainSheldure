using System.Threading.Tasks;

namespace Trains.Resources
{
    public interface ILocalDataService
    {
        Task<T> GetData<T>(string filename, string lang) where T : class;
    }
}
