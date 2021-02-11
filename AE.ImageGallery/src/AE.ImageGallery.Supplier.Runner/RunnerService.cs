using System;
using System.Threading;
using System.Threading.Tasks;
using AE.ImageGallery.Supplier.Application.Api;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AE.ImageGallery.Supplier.Runner
{
    public class RunnerService: IHostedService
    {
        private readonly IImageGalleryClient _client;
        private readonly ILogger<RunnerService> _logger;

        public RunnerService(
            IImageGalleryClient client,
            ILogger<RunnerService> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                var images = await _client.GetImages();
                _logger.LogInformation($"{images.PageCount}");
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}