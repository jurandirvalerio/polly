using System.Net.Http;
using System.Threading.Tasks;

namespace ApiCore
{
	public interface IHttpClientAdapter
	{
		Task<HttpResponseMessage> SendAsync(HttpRequestMessage httpRequestMessage);
	}
}