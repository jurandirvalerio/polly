using System.Threading.Tasks;

namespace CircuitBreaker.Api.Services
{
	public interface IExemploCircuitoService
	{
		Task<string> BuscarDado();
	}
}