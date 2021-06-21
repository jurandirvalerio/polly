using System;
using System.Net;

namespace CircuitBreaker.Api.Services
{
	public class CustomApiException : Exception
	{
		public HttpStatusCode StatusCode { get; }

		public CustomApiException(HttpStatusCode statusCode)
		{
			StatusCode = statusCode;
		}
	}
}