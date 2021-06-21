using System;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Polly;
using Polly.Caching;
using Polly.Registry;

namespace Cache.Api
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers();

			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "Cache.Api", Version = "v1" });
			});

			services.AddMemoryCache();

			services.AddSingleton<Polly.Caching.IAsyncCacheProvider, Polly.Caching.Memory.MemoryCacheProvider>();
			services.AddSingleton<Polly.Registry.IReadOnlyPolicyRegistry<string>, Polly.Registry.PolicyRegistry>((serviceProvider) =>
			{
				PolicyRegistry registry = new PolicyRegistry();
				registry.Add("cacheTokenPolicy",
					Policy.CacheAsync<HttpResponseMessage>(
						serviceProvider
							.GetRequiredService<IAsyncCacheProvider>()
							.AsyncFor<HttpResponseMessage>(),
						TimeSpan.FromSeconds(10)));
				return registry;
			});
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cache.Api v1"));
			}

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
