using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trains.Infrastructure.Interfaces;
using Trains.Services.Interfaces;

namespace Trains.Services.Implementations
{
	public class TestService : ITestService
	{
		private readonly IHttpService _httpService;

		public TestService(IHttpService httpService)
		{
			_httpService = httpService;
		}

		public async Task<string> GetHtml(Uri uri)
		{
			return await _httpService.LoadResponseAsync(uri);
		}
	}
}
