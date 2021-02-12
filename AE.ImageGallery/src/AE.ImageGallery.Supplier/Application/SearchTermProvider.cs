using System.Threading.Tasks;
using AE.ImageGallery.Supplier.Application.Api;

namespace AE.ImageGallery.Supplier.Application
{
    public class SearchTermProvider : ISearchTermProvider
    {
        private readonly IImageGalleryService _imageGalleryService;
        private readonly ISearchTermService _searchTermService;

        public SearchTermProvider(IImageGalleryService imageGalleryService, ISearchTermService searchTermService)
        {
            _imageGalleryService = imageGalleryService;
            _searchTermService = searchTermService;
        }

        public async Task<SearchTermsOnPage> GetSearchTermsOnPage(int pageNumber)
        {
            var imagesOnPage = await _imageGalleryService.GetImagesOnPage(pageNumber);
            var searchTermsOnPage = _searchTermService.GetSearchTermsOnPage(imagesOnPage);

            return searchTermsOnPage;
        }
    }
}