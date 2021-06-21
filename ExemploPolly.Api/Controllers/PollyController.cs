using System.Threading.Tasks;
using CircuitBreaker.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace CircuitBreaker.Api.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class PollyController : ControllerBase
	{
		#region Ações
		
		[HttpGet("CircuitBreaker")]
		public async Task<IActionResult> CircuitBreaker()
		{
			return Ok(await _exemploCircuitoService.BuscarDado());
		}
	
		#endregion

		#region Campos

		private readonly IExemploCircuitoService _exemploCircuitoService;

		#endregion

		#region Construtores

		public PollyController(IExemploCircuitoService exemploCircuitoService)
		{
			_exemploCircuitoService = exemploCircuitoService;
		}

		#endregion
	}
}