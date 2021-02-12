using System.Collections.Generic;
using System.Linq;
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

        public async Task<List<SearchTerm>> GetSearchTerms()
        {
            var firstPage = 1;
            var resultFromFirstPage = await GetImageTermsOnPage(firstPage);
            var termsFromFirstPage = resultFromFirstPage.terms;
            var lastPage = resultFromFirstPage.totalPages;
            var termsFromOtherPages = await GetImageTermsBetweenPages(firstPage, lastPage);

            var terms = _searchTermService.CombineSearchTerms(termsFromFirstPage, termsFromOtherPages);

            return terms;
        }

        private async Task<(List<SearchTerm> terms, int totalPages)> GetImageTermsOnPage(int pageNumber)
        {
            var imagesOnPage = await _imageGalleryService.GetImagesOnPage(pageNumber);
            var searchTerms = _searchTermService.GetSearchTerms(imagesOnPage);

            return (searchTerms, imagesOnPage.PageCount);
        }

        private async Task<List<SearchTerm>> GetImageTermsBetweenPages(int fromPageExclude, int toPageInclude)
        {
            if (fromPageExclude >= toPageInclude)
                return new List<SearchTerm>();

            var currentPageNumber = toPageInclude;
            var getImageTermsTasks = new List<Task<(List<SearchTerm> terms, int totalPages)>>();
            do
            {
                var task = this.GetImageTermsOnPage(currentPageNumber);
                getImageTermsTasks.Add(task);
                currentPageNumber--;
            } while (currentPageNumber != fromPageExclude);

            var resultFromOtherPages = (await Task.WhenAll(getImageTermsTasks)).ToList();
            var result = resultFromOtherPages.SelectMany(x => x.terms).ToList();

            return result;
        }


    }
}