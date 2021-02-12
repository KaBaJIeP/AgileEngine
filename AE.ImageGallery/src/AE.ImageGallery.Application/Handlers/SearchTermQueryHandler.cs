using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AE.ImageGallery.Application.Api;
using AE.ImageGallery.Application.Models;
using MediatR;

namespace AE.ImageGallery.Application.Handlers
{
    public class SearchTermQueryHandler : IRequestHandler<GetImagesBySearchTermQuery, List<ImageModel>>
    {
        private readonly ISearchTermRepository _searchTermRepository;
        private readonly IImageRepository _imageRepository;

        public SearchTermQueryHandler(
            ISearchTermRepository searchTermRepository,
            IImageRepository imageRepository)
        {
            _searchTermRepository = searchTermRepository;
            _imageRepository = imageRepository;
        }

        public async Task<List<ImageModel>> Handle(GetImagesBySearchTermQuery request, CancellationToken cancellationToken)
        {
            var imageIds = await _searchTermRepository.GetImageIds(request.SearchTerm);
            var images = await _imageRepository.GetImages(imageIds);
            return images;
        }
    }
}