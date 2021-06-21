using System;
using System.Net.Http;
using System.Threading.Tasks;
using ApiCore;
using Microsoft.AspNetCore.Mvc;
using Polly;
using Polly.Retry;

namespace RetryApi.Controllers
{
	public class RetryController : ControllerBase
	{
		#region Ações

		[HttpGet("EfetuarRequisicaoSemPolly")]
		public async Task<IActionResult> EfetuarRequisicaoSemPolly()
		{
			HttpRequestMessage httpRequestMessage = HttpRequestMessageFactory.Criar();
			HttpResponseMessage requisicao = await _httpClientAdapter.SendAsync(httpRequestMessage);
			LogService.Logar(ObterMensagemStatusRequisicao(requisicao));
			return Retorno(requisicao);
		}

		[HttpGet("TentarDezVezes")]
		public async Task<IActionResult> TentarDezVezes()
		{
			LogarDivisao();

			AsyncRetryPolicy<HttpResponseMessage> policy = ObterPoliticaTentarDezVezes();
			HttpResponseMessage httpResponseMessage = await policy.ExecuteAsync(async() => await _httpClientAdapter.SendAsync(HttpRequestMessageFactory.Criar()));

			return Retorno(httpResponseMessage);
		}

		[HttpGet("TentarEternamente")]
		public async Task<IActionResult> TentarEternamente()
		{
			LogarDivisao();
			AsyncRetryPolicy<HttpResponseMessage> policy = ObterPoliticaTentarEternamente();
			HttpResponseMessage httpResponseMessage = await policy.ExecuteAsync(() => _httpClientAdapter.SendAsync(HttpRequestMessageFactory.Criar()));

			return Retorno(httpResponseMessage);
		}

		#endregion

		#region Campos
		private readonly IHttpClientAdapter _httpClientAdapter;
		#endregion

		#region Construtor
		public RetryController(IHttpClientAdapter httpClientAdapter)
		{
			_httpClientAdapter = httpClientAdapter;
		}
		#endregion

		#region Métodos
		private IActionResult Retorno(HttpResponseMessage responseMessage)
		{
			string mensagem = ObterMensagemStatusRequisicao(responseMessage);
			LogService.Logar(mensagem);
			LogarDivisao();
			LogService.Logar(Environment.NewLine);
			return responseMessage.IsSuccessStatusCode ? (IActionResult) Ok(mensagem) : BadRequest(mensagem);
		}

		private static string ObterMensagemStatusRequisicao(HttpResponseMessage responseMessage)
		{
			return responseMessage.IsSuccessStatusCode ? "Ok" : "Solicitação falhou";
		}

		private void LogarDivisao() => LogService.Logar("===============================================");
		#endregion

		#region Políticas

		public AsyncRetryPolicy<HttpResponseMessage> ObterPoliticaTentarEternamente()
		{

			return Policy
				.HandleResult<HttpResponseMessage>(message => !message.IsSuccessStatusCode)
				.WaitAndRetryForeverAsync(
					retryAttempt => TimeSpan.FromSeconds(3),
					(exception, timespan, context) =>
					{
						LogService.Logar("Erro. Tentarei novamente...");
					});
		}

		public AsyncRetryPolicy<HttpResponseMessage> ObterPoliticaTentarDezVezes()
		{
			return Policy
				.HandleResult<HttpResponseMessage>(message => !message.IsSuccessStatusCode)
				.WaitAndRetryAsync(new[]
				{
					TimeSpan.FromSeconds(1),
					TimeSpan.FromSeconds(2),
					TimeSpan.FromSeconds(3),
					TimeSpan.FromSeconds(4),
					TimeSpan.FromSeconds(5),
					TimeSpan.FromSeconds(6),
					TimeSpan.FromSeconds(7),
					TimeSpan.FromSeconds(8),
					TimeSpan.FromSeconds(9),
					TimeSpan.FromSeconds(10),
				}, (outcome, timeSpan, retryCount, context) =>
				{
					LogService.Logar($"Tentativa {retryCount}");
				});
		}

		#endregion
	}
}