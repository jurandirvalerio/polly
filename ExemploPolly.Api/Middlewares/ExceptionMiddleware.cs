using System.Net;
using System.Threading.Tasks;
using CircuitBreaker.Api.Services;
using Microsoft.AspNetCore.Http;
using Polly.CircuitBreaker;

namespace CircuitBreaker.Api.Middlewares
{
	public class ExceptionMiddleware
	{
		private readonly RequestDelegate _next;

		public ExceptionMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext httpContext)
		{
			try
			{
				await _next(httpContext);
			}
			catch (CustomApiException ex)
			{
				HandleRequestExceptionAsync(httpContext, ex.StatusCode);
			}
			catch (BrokenCircuitException)
			{
				HandleCircuitBreakerExceptionAsync(httpContext);
			}
		}

		private static void HandleCircuitBreakerExceptionAsync(HttpContext context)
		{
			context.Response.WriteAsync("Circuit breaker aberto!");
		}

		private static void HandleRequestExceptionAsync(HttpContext context, HttpStatusCode statusCode)
		{
			context.Response.StatusCode = (int)statusCode;
		}
	}
}