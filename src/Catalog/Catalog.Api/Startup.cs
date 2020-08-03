using Catalog.Api.Data;
using Catalog.Api.Data.Interfaces;
using Catalog.Api.Repositories;
using Catalog.Api.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;

namespace Catalog.Api
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
            services.Configure<CatalogDatabaseSettings>(Configuration.GetSection(nameof(CatalogDatabaseSettings)));
            services.AddSingleton<ICatalogDatabaseSettings>(sp => sp.GetRequiredService<IOptions<CatalogDatabaseSettings>>().Value);
            services.AddTransient<ICatalogContext, CatalogContext>();
            services.AddTransient<ICatalogRepository, CatalogRepository>();
            services.AddApiVersioning(apiVerConfig =>
                                            {
                                                apiVerConfig.AssumeDefaultVersionWhenUnspecified = true;
                                                apiVerConfig.DefaultApiVersion = new ApiVersion(new DateTime(2020, 6, 6));
                                            });
            services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = "Microservice - Order Web API",
                Version = "v1",
                Description = "Sample microservice for order",
            });
        });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "PlaceInfo Services"));
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
