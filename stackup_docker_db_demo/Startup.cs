using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.OpenApi.Models;
using stackup_docker_db_demo.DBProviders;

namespace stackup_docker_db_demo
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers();
			var mongoConnectionString = Configuration.GetValue<string>("DatabaseSettings:MongoConnectionString");
			var postgressConnectionString = Configuration.GetValue<string>("DatabaseSettings:PostgressConnectionString");
			var redisConnectionString = Configuration.GetValue<string>("CacheSettings:RedisCache");


			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo
				{
					Version = "v1"
				});
			});

			services.AddSingleton<IMongoClient>(x =>
			{
				return new MongoClient(mongoConnectionString);
			});
			services.AddStackExchangeRedisCache(options =>
			{
				options.Configuration = redisConnectionString;
			});
			services.AddSingleton<RedisProvider>();
			services.AddSingleton<MongoProvider>();
			services.AddSingleton<PostgressProvider>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseSwagger();
			app.UseSwaggerUI(c => {
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "Showing API V1");
			});
			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
