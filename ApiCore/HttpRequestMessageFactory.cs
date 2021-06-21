using System;
using System.Net.Http;

namespace ApiCore
{
	public class HttpRequestMessageFactory
	{
		public static HttpRequestMessage Criar(string action = "requisicao")
		{
			var requestMessage = new HttpRequestMessage
			{
				Method = HttpMethod.Get,
				RequestUri = new Uri($"http://localhost:55555/apiexemplo/{action}")

			};

			requestMessage.Headers.Clear();
			return requestMessage;
		}
	}
}