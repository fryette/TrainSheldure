using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using Trains.Infrastructure.Interfaces;
using Trains.Models;

namespace Trains.Web.Controllers
{
	public class ValuesController : ApiController
	{
		private readonly ISearchService _searchService;
		private IAppSettings _appSettings;
		public ValuesController(ISearchService searchService1)
		{
			_searchService = searchService1;
		}

		// GET api/values
		public async Task<HttpResponseMessage> Get(string from,string to,string lang)
		{
			string line = "";
			using (var sr = new StreamReader(HttpContext.Current.Server.MapPath("/Resources/ru/Countries/Belarus.json")))
			{
				// Read the stream to a string, and write the string to the console.
				line = sr.ReadToEnd();
			}
			var list = JsonConvert.DeserializeObject<List<CountryStopPointItem>>(line);
			var result =
				await
					_searchService.GetTrainSchedule(list.First(x => x.Value == "Берёза-Город"), list.First(x => x.Value == "Брест"),
						DateTimeOffset.Now, "1");
			return new HttpResponseMessage()
			{
				Content = new StringContent(JsonConvert.SerializeObject(result), System.Text.Encoding.UTF8, "text/html")
			};
		}

		// GET api/values/5
		public string Get(int id)
		{
			return "value";
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
