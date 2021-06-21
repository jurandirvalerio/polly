using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApiCore
{
	public class HttpClientAdapter : IHttpClientAdapter
	{
		private readonly HttpClient _httpClient;

		public HttpClientAdapter()
		{
			_httpClient = new HttpClient
			{
				Timeout = TimeSpan.FromSeconds(30)
			};
		}

		public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage httpRequestMessage) => await _httpClient.SendAsync(httpRequestMessage);
	}
}