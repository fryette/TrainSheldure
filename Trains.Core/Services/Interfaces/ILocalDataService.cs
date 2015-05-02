using System.Threading.Tasks;
using Trains.Core.Interfaces;

namespace Trains.Core.Services.Interfaces
{
    public interface ILocalDataService
    {
        Task<T> GetLanguageData<T>(string jsonText) where T : class;
        Task<IPattern> GetPatterns();
    }
}
