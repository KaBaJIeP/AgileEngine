using System;
using System.Threading;
using System.Threading.Tasks;
using AE.ImageGallery.Supplier.Application;
using AE.ImageGallery.Supplier.Application.Api;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AE.ImageGallery.Supplier.Runner
{
    public class RunnerService: IHostedService
    {
        private readonly IImageGalleryService _service;
        private readonly ILogger<RunnerService> _logger;

        public RunnerService(
            IImageGalleryService service,
            ILogger<RunnerService> logger)
        {
            _service = service;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                var pageCount = 0;
                var currentPage = 1;
                do
                {
                    var imagesOnPage = await _service.GetImagesOnPage(currentPage);
                    pageCount = imagesOnPage.PageCount;
                    currentPage++;
                } while (currentPage <= pageCount);
            }
            catch (AggregateException aex)
            {
                var e = aex.Flatten();
                _logger.LogError(e, e.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}