using System;
using System.Threading.Tasks;

namespace Trains.Infrastructure.Interfaces
{
	public interface IHttpService
	{
		Task<string> LoadResponseAsync(Uri uri, string method = "GET", string contentType = "text/html");
	}
}