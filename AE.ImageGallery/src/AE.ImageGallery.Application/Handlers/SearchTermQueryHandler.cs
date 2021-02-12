using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AE.ImageGallery.Application.Api;
using AE.ImageGallery.Application.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AE.ImageGallery.Application.Handlers
{
    public class SearchTermQueryHandler : IRequestHandler<GetImagesBySearchTermQuery, List<ImageModel>>
    {
        private readonly ISearchTermRepository _searchTermRepository;
        private readonly ILogger<SearchTermQueryHandler> _logger;

        public SearchTermQueryHandler(ISearchTermRepository searchTermRepository, ILogger<SearchTermQueryHandler> logger)
        {
            _searchTermRepository = searchTermRepository;
            _logger = logger;
        }

        public async Task<List<ImageModel>> Handle(GetImagesBySearchTermQuery request, CancellationToken cancellationToken)
        {
            var imageIds = await _searchTermRepository.GetImageIds(request.SearchTerm);
            _logger.LogInformation($"{string.Join(",",imageIds)}");
            return new List<ImageModel>();
        }
    }
}