using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Trains.Core.Resources;
using Trains.Core.Services.Interfaces;
using Trains.Model.Entities;

namespace Trains.Core.Services
{
	public class BackendService : IBackendService
	{
		private readonly IHttpService _httpService;

		public BackendService(IHttpService httpService)
		{
			_httpService = httpService;
		}

		public async Task<ChygunkaSettings> GetAppSettings()
		{
			string response = await _httpService.LoadResponseAsync(new Uri(String.Format("{0}{1}", Constants.BaseAppUri, Constants.SettingsRelativeUri)));
			var result = JsonConvert.DeserializeObject<ChygunkaSettings>(response);
			return result;
		}
	}
}

