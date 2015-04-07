using Cirrious.MvvmCross.Plugins.Network.Rest;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Trains.Infrastructure.Interfaces;
using Trains.Services.Interfaces;

namespace Trains.Core
{
    public class BaseHttpService : MvxRestClient, IHttpService
    {
        public async Task<string> LoadResponseAsync(Uri uri, string contentType = "text/html")
        {
            var request = new MvxRestRequest(uri: uri, accept: contentType);
            var httpWebRequest = BuildHttpRequest(request);
            var response = await ExecuteRequestAsync(httpWebRequest);

            using (var stream = response.GetResponseStream())
            {
                using (var reader = new StreamReader(stream))
                {
                    return await reader.ReadToEndAsync();
                }
            }
        }

        public async Task<string> LoadDataResponseAsync(Uri uri, string body = "", string contentType = "application/json")
        {
            var request = new MvxStringRestRequest(uri: uri, body: body, accept: contentType);
            var httpWebRequest = BuildHttpRequest(request);
            var response = await ExecuteRequestAsync(httpWebRequest);

            using (var stream = response.GetResponseStream())
            {
                using (var reader = new StreamReader(stream))
                {
                    return await reader.ReadToEndAsync();
                }
            }
        }

        protected virtual async Task<WebResponse> ExecuteRequestAsync(HttpWebRequest request, string data = null)
        {
            if (data != null)
            {
                var bytes = Encoding.UTF8.GetBytes(data);

                try
                {
                    var requestStream = await request.GetRequestStreamAsync();
                    requestStream.Write(bytes, 0, bytes.Length);
                    requestStream.Dispose();
                }
                catch (Exception ex)
                {
                    var webException = ex as WebException;
                    if (webException != null)
                    {
                        var response = webException.Response as HttpWebResponse;
                        if (response != null && response.StatusCode == HttpStatusCode.NotFound)
                        {
                            throw;
                        }

                        throw;
                    }
                }
            }
            var req = await request.GetResponseAsync();
            return req;
        }

    }
}
