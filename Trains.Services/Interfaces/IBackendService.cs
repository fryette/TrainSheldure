using System.Threading.Tasks;
using Trains.Model.Entities;

namespace Trains.Services
{
    public interface IBackendService
    {

        Task<ChygunkaSettings> GetAppSettings();

    }
}

