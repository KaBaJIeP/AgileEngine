using System;
using System.Linq;
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
        private readonly ILogger<RunnerService> _logger;

        public RunnerService(
            ISearchTermProvider searchTermProvider,
            ILogger<RunnerService> logger)
        {
            _searchTermProvider = searchTermProvider;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                var terms = await _searchTermProvider.GetSearchTerms();

                // imagesPerPage termsPerPage
                // save imagesPerPage to db
                // save termsPerPage to cache

                _logger.LogInformation($"{string.Join(",",terms.Select(x => x.Term))}");
                foreach (var term in terms)
                {
                    _logger.LogInformation($"{term.Term} : {string.Join(",",term.PictureIds)}");
                }
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