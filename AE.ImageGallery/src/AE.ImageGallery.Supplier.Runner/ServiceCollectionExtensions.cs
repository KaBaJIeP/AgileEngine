using System.Collections.Generic;
using AE.ImageGallery.Supplier.Application;
using AE.ImageGallery.Supplier.Application.Api;
using AE.ImageGallery.Supplier.Configs;
using AE.ImageGallery.Supplier.Infrastructure;
using AE.ImageGallery.Supplier.Infrastructure.Mapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RestEase.HttpClientFactory;

namespace AE.ImageGallery.Supplier.Runner
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureRunner(this IServiceCollection collection, IConfiguration configuration)
        {
            collection.AddAutoMapper((cfg =>
            {
                cfg.AddProfile(new MongoDbProfile());
            }));

            collection.AddHttpClient();
            collection.Configure<AgileEngineConfig>(configuration.GetSection(AgileEngineConfig.SectionName));

            var config = configuration.GetSection(AgileEngineConfig.SectionName).Get<AgileEngineConfig>();
            collection.AddRestEaseClient<IImageGalleryApi>(config.ApiUrl);

            collection.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = config.RedisConnectionString;
            });

            collection.AddScoped<IImageGalleryApiClient, ImageGalleryApiClient>();
            collection.AddScoped<IImageGalleryService, ImageGalleryService>();
            collection.AddScoped<IEqualityComparer<SearchTerm>, SearchTermComparer>();
            collection.AddScoped<ISearchTermProvider, SearchTermProvider>();
            collection.AddScoped<ISearchTermService, SearchTermService>();
            collection.AddScoped<IImageRepository, ImageRepository>();
            collection.AddScoped<ISearchTermsRepository, SearchTermsRepository>();

            return collection;
        }
    }
}