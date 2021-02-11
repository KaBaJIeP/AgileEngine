using AE.ImageGallery.Supplier.Application;
using AE.ImageGallery.Supplier.Application.Api;
using AE.ImageGallery.Supplier.Runner.Configs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RestEase.HttpClientFactory;

namespace AE.ImageGallery.Supplier.Runner
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureRunner(this IServiceCollection collection, IConfiguration configuration)
        {
            collection.AddHttpClient();
            collection.Configure<AgileEngineConfig>(configuration.GetSection(AgileEngineConfig.SectionName));
            var config = configuration.GetSection(AgileEngineConfig.SectionName).Get<AgileEngineConfig>();
            collection.AddRestEaseClient<IImageGalleryApi>(config.ApiUrl);

            collection.AddSingleton<IImageGalleryClient, ImageGalleryClient>();
            return collection;
        }
    }
}