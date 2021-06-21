using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExemploPolly
{
    public class ApiService : IApiService
    {
        private readonly ILogger _logger;
        private readonly HttpClient _httpClient;

        public ApiService(
            HttpClient httpClient,
            ILogger<ApiService> logger)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        public async Task ObterNumerosAsync()
        {
            HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync("Polly/Numeros");

            _logger.LogInformation($"StatusCode: {httpResponseMessage.StatusCode:D}");
        }
    }
}
