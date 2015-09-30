using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Trains.Infrastructure.Interfaces;

namespace Trains.Infrastructure
{
	public class BaseHttpService : IHttpService
	{
		public async Task<string> LoadResponseAsync(Uri uri, string contentType = "text/html")
		{
			//var request = new MvxRestRequest(uri, accept: contentType);
			//var httpWebRequest = BuildHttpRequest(request);
			//WebResponse response;
			//try
			//{
			//	response = await ExecuteRequestAsync(httpWebRequest);
			//}
			//catch (Exception)
			//{
			//	return null;
			//}

			//using (var stream = response.GetResponseStream())
			//{
			//	using (var reader = new StreamReader(stream))
			//	{
			//		return await reader.ReadToEndAsync();
			//	}
			//}
			return "";

		}

		public async Task<string> LoadDataResponseAsync(Uri uri, string body = "", string contentType = "application/json")
		{
			//var request = new MvxStringRestRequest(uri, body, accept: contentType);
			//var httpWebRequest = BuildHttpRequest(request);
			//var response = await ExecuteRequestAsync(httpWebRequest);

			//using (var stream = response.GetResponseStream())
			//{
			//	using (var reader = new StreamReader(stream))
			//	{
			//		return await reader.ReadToEndAsync();
			//	}
			//}
			return "";

		}

		protected virtual async Task<WebResponse> ExecuteRequestAsync(HttpWebRequest request, string data = null)
		{
			//if (data != null)
			//{
			//	var bytes = Encoding.UTF8.GetBytes(data);

			//	try
			//	{
			//		var requestStream = await request.GetRequestStreamAsync();
			//		requestStream.Write(bytes, 0, bytes.Length);
			//		requestStream.Dispose();
			//	}
			//	catch (Exception ex)
			//	{
			//		var webException = ex as WebException;
			//		if (webException != null)
			//		{
			//			var response = webException.Response as HttpWebResponse;
			//			if (response != null && response.StatusCode == HttpStatusCode.NotFound)
			//			{
			//				throw;
			//			}

			//			throw;
			//		}
			//	}
			//}
			//var req = await request.GetResponseAsync();
			//return req;
			return null;

		}

		public async Task<string> Post(Uri url, List<KeyValuePair<string, string>> values,  Encoding encoding)
        {
            //var content = new Extensions.FormUrlEncodedContent(values);
            //var httpClient = new HttpClient(new HttpClientHandler());
            //var response = await httpClient.PostAsync(url, content);
            //var temp = await response.Content.ReadAsByteArrayAsync();
            //return encoding.GetString(temp, 0, temp.Length - 1);
	        return "";
        }
    }
}
