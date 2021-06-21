using System;
using System.Data;
using ApiCore;
using Microsoft.AspNetCore.Mvc;

namespace ApiDestino.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class ApiExemploController : ControllerBase
	{
		private readonly IConfiguracaoService _configuracaoService;

		public ApiExemploController(IConfiguracaoService configuracaoService)
		{
			_configuracaoService = configuracaoService;
		}

		[HttpGet("Requisicao")]
		public IActionResult Requisicao()
		{
			LogService.Logar($"Requisição recebida em: {nameof(Requisicao)}");

			if(_configuracaoService.ErroHabilitado) return BadRequest("BadRequest");
			return Ok("Ok");
		}

		[HttpGet("RequisicaoPorParametro")]
		public IActionResult RequisicaoPorParametro(string id)
		{
			LogService.Logar($"Requisição recebida em: {nameof(RequisicaoPorParametro)}");
			return Ok(Guid.NewGuid());
		}

		[HttpGet("RequisicaoPassivelErro")]
		public IActionResult RequisicaoPassivelErro()
		{
			LogService.Logar($"Requisição recebida em: {nameof(RequisicaoPassivelErro)}");
			return _configuracaoService.ErroHabilitado ? throw new DataException("Erro na conexão com o banco de dados"): Ok("Ok");
		}

		[HttpGet("habilitarErros")]
		public IActionResult AlterarEstadoErros()
		{
			LogService.Logar($"Requisição recebida em: {nameof(AlterarEstadoErros)}");
			_configuracaoService.ErroHabilitado = !_configuracaoService.ErroHabilitado;
			return Ok($"Status: {ObterStatusErro()}");
		}

		private string ObterStatusErro() => _configuracaoService.ErroHabilitado ? "erro habilitado" : "erro desabilitado";
	}
}