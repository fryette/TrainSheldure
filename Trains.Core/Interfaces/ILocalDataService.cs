using System.Threading.Tasks;

namespace Trains.Core.Interfaces
{
    public interface ILocalDataService
    {
        Task<T> GetData<T>(string filename) where T : class;
    }
}
