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
        private readonly ISearchTermProvider _searchTermProvider;
        private readonly IImageRepository _imageRepository;
        private readonly ILogger<RunnerService> _logger;

        public RunnerService(
            ISearchTermProvider searchTermProvider,
            IImageRepository imageRepository,
            ILogger<RunnerService> logger)
        {
            _searchTermProvider = searchTermProvider;
            _imageRepository = imageRepository;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                await _imageRepository.DeleteAllImages();

                var currentPage = 1;
                var hasMoreImages = false;
                do
                {
                    var result = await _searchTermProvider.GetSearchTermsOnPage(currentPage);
                    hasMoreImages = result.ImagesOnPage.HasMore;
                    currentPage++;

                    await _imageRepository.Save(result.ImagesOnPage);
                    // save termsPerPage to cache
                } while (hasMoreImages);
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