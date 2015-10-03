using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;
using Trains.Infrastructure;
using Trains.Infrastructure.Interfaces;

namespace Trains.Web.Controllers
{
	public class TrainController : ApiController
	{
		private readonly ISearchService _searchService;

		public TrainController(ISearchService searchService)
		{
			_searchService = searchService;
		}

		public async Task<HttpResponseMessage> Get(string fromEcp, string toEcp, string date)
		{
			var result =
				await
					_searchService.GetTrainSchedule(Countries.CountriesList.First(x => x.Ecp == fromEcp), Countries.CountriesList.First(x => x.Ecp == fromEcp),
						date);
			return new HttpResponseMessage
			{
				Content = new StringContent(JsonConvert.SerializeObject(result), System.Text.Encoding.UTF8, "text/html")
			};
		}

		// POST api/values
		public void Post([FromBody]string value)
		{
		}

		// PUT api/values/5
		public void Put(int id, [FromBody]string value)
		{
		}

		// DELETE api/values/5
		public void Delete(int id)
		{
		}

	}
}