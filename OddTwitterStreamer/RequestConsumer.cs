using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace OddTwitterStreamer
{
    public class RequestConsumer
    {
        public async Task<Stream> GetRequestContentStreamAsync(Uri requestUri)
        {
            HttpResponseMessage response = await MakeGetHttpRequestAsync(requestUri);

            return await response.Content.ReadAsStreamAsync();
        }

        public async Task<string> GetRequestContentStringAsync(Uri requestUri)
        {
            HttpResponseMessage response = await MakeGetHttpRequestAsync(requestUri);

            return await response.Content.ReadAsStringAsync();
        }

        async Task<HttpResponseMessage> MakeGetHttpRequestAsync(Uri requestUri)
        {
            using (var httpClient = new HttpClient() { Timeout = TimeSpan.FromMinutes(5) })
            {
                var requestMessage = new HttpRequestMessage();

                requestMessage.RequestUri = requestUri;
                requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
                requestMessage.Method = HttpMethod.Get;
                HttpResponseMessage message = await httpClient.SendAsync(requestMessage).ConfigureAwait(false);

                return message;
            }
        }
    }
}
