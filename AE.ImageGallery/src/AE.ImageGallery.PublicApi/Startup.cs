using AE.ImageGallery.Application.Api;
using AE.ImageGallery.Application.Controllers;
using AE.ImageGallery.Infrastructure;
using AE.ImageGallery.Supplier.Configs;
using AE.ImageGallery.Supplier.Infrastructure.Mapper;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace AE.ImageGallery.PublicApi
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
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "AE.ImageGallery.PublicApi", Version = "v1"});
            });

            ConfigureApplication(services);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AE.ImageGallery.PublicApi v1"));
            }

            app.UseRouting();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        private void ConfigureApplication(IServiceCollection services)
        {
            services.AddAutoMapper((cfg =>
            {
                cfg.AddProfile(new MongoDbProfile());
            }));

            services.Configure<AgileEngineConfig>(Configuration.GetSection(AgileEngineConfig.SectionName));
            var config = Configuration.GetSection(AgileEngineConfig.SectionName).Get<AgileEngineConfig>();
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = config.RedisConnectionString;
            });

            services.AddSingleton<ISearchTermRepository, SearchTermRepository>();
            services.AddSingleton<IImageRepository, ImageRepository>();
            services.AddMediatR(typeof(SearchController));
        }
    }
}