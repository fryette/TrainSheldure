using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Trains.Core.Services.Interfaces
{
	public interface IHttpService
	{
		Task<string> LoadResponseAsync(Uri uri, string contentType = "text/html");
		Task<string> LoadDataResponseAsync(Uri uri, string body = "", string contentType = "application/json");

	    Task<String> Post(string url, Dictionary<string, string> headers = null, string body = "",
	        string contentType = "application/json");
	}
}