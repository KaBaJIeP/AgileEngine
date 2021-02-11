using System;
using System.Threading;
using System.Threading.Tasks;
using AE.ImageGallery.Supplier.Application.Api;
using AE.ImageGallery.Supplier.Runner.Configs;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace AE.ImageGallery.Supplier.Runner
{
    public class RunnerService: IHostedService
    {
        private readonly IImageGalleryClient _client;
        private readonly IOptions<AgileEngineConfig> _config;

        public RunnerService(IImageGalleryClient client, IOptions<AgileEngineConfig> config)
        {
            _client = client;
            _config = config;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var authResponse = await _client.Auth(_config.Value.ApiKey);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}