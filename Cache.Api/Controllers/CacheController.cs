using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Polly;
using Polly.Registry;

namespace Cache.Api.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class CacheController : ControllerBase
	{
		private readonly IReadOnlyPolicyRegistry<string> _policyRegistry;

		#region Ações
		
		[HttpGet("Cache")]
		public async Task<IActionResult> Cache(string id)
		{
			var context = new Context($"Token{id}");
			var cachePolicy = _policyRegistry.Get<IAsyncPolicy<HttpResponseMessage>>("cacheTokenPolicy");

			var httpResponseMessage = await cachePolicy.ExecuteAsync(
				(ctx) => new HttpClient().SendAsync(ObterHttpRequestMessage(id)), context
				);

			return Ok(new
			{
				Guid = httpResponseMessage.Content.ReadAsStringAsync().Result,
				Data = DateTime.Now
			});
		}
	
		#endregion

		#region Campos


		#endregion

		#region Construtores

		public CacheController(IReadOnlyPolicyRegistry<string> policyRegistry)
		{
			_policyRegistry = policyRegistry;
		}

		private static HttpRequestMessage ObterHttpRequestMessage(string id)
		{
			var requestMessage = new HttpRequestMessage
			{
				Method = HttpMethod.Get,
				RequestUri = new Uri($"http://localhost:55555/apiexemplo/requisicaoPorParametro?id={id}")
			};

			requestMessage.Headers.Clear();
			return requestMessage;
		}

		#endregion
	}
}