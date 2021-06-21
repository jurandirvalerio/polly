using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExemploPolly
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            #region Dependency Injection

            IServiceCollection serviceCollection = new ServiceCollection();

            serviceCollection.AddLogging(options =>
            {
                options.AddSimpleConsole();
            });

            serviceCollection
                .AddHttpClient<IApiService, ApiService>(client =>
                {
                    client.BaseAddress = new Uri("https://localhost:5001/");
                })
                .AddPolicyHandler(GetRetryPolicy());

            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            IApiService apiService = serviceProvider.GetService<IApiService>();

            #endregion Dependency Injection

            await apiService.ObterNumerosAsync();

            Console.ReadKey();
        }

        static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.BadRequest)
                .WaitAndRetryAsync(new[] 
                    {
                        TimeSpan.FromSeconds(1),
                        TimeSpan.FromSeconds(2),
                        TimeSpan.FromSeconds(3),
                    });
        }

        private static void Retry()
        {
            Policy
                .Handle<ApplicationException>()
                .Retry(3)
                .Execute(Tarefa);
        }

        private static void WaitAndRetry()
        {
            Policy
                .Handle<ApplicationException>()
                .WaitAndRetry(new[]
                {
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(2)
                })
                .Execute(Tarefa);
        }

        private static void RetryForever()
        {
            Policy
                .Handle<ApplicationException>()
                .RetryForever()
                .Execute(Tarefa);
        }

        private static void Tarefa()
        {
            throw new ApplicationException();
        }
    }
}
