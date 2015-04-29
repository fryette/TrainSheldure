using System.Threading.Tasks;
using Trains.Model.Entities;

namespace Trains.Core.Services
{
    public interface IBackendService
    {

        Task<ChygunkaSettings> GetAppSettings();

    }
}

