using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AE.ImageGallery.Supplier.Application.Api;
using AE.ImageGallery.Supplier.Application.Contracts;

namespace AE.ImageGallery.Supplier.Application
{
    public class SearchTermProvider : ISearchTermProvider
    {
        private readonly IImageGalleryService _service;
        private readonly IEqualityComparer<SearchTerm> _comparer;

        public SearchTermProvider(IImageGalleryService service, IEqualityComparer<SearchTerm> comparer)
        {
            _service = service;
            _comparer = comparer;
        }

        public async Task<List<SearchTerm>> GetSearchTerms()
        {
            var firstPage = 1;
            var resultFromFirstPage = await GetImageTermsOnPage(firstPage);
            var termsFromFirstPage = resultFromFirstPage.terms;
            var lastPage = resultFromFirstPage.totalPages;
            var termsFromOtherPages = await GetImageTermsBetweenPages(firstPage, lastPage);

            var terms = this.CombineSearchTerms(termsFromFirstPage, termsFromOtherPages);

            return terms;
        }

        private async Task<(List<SearchTerm> terms, int totalPages)> GetImageTermsOnPage(int pageNumber)
        {
            var imagesOnPage = await _service.GetImagesOnPage(pageNumber);
            var searchTerms = this.GetSearchTerms(imagesOnPage);

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

        private List<SearchTerm> GetSearchTerms(ImagesOnPage imagesOnPage)
        {
            var imageSearchTerms = imagesOnPage.Pictures.Select(this.MapToTerms)
                .SelectMany(x => x)
                .ToList();
            var result = this.ReduceTerms(imageSearchTerms);

            return result;
        }

        private List<SearchTerm> MapToTerms(PictureResponseDto picture)
        {
            var authorTerms = GetTerms(picture.Author, picture.Id);
            var cameraTerms = GetTerms(picture.Camera, picture.Id);
            var tagsTerms = GetTerms(picture.Tags, picture.Id);

            var result = new List<SearchTerm>();
            result.AddRange(authorTerms);
            result.AddRange(cameraTerms);
            result.AddRange(tagsTerms);

            return result.Distinct(_comparer).ToList();
        }

        private List<SearchTerm> ReduceTerms(List<SearchTerm> searchTerms)
        {
            var map = searchTerms.GroupBy(x => x.Term)
                .ToDictionary(x => x.Key,
                    y => y.Select(z => z.PictureIds)
                        .SelectMany(ids => ids).Distinct().ToList());
            var result = map.Select(x => new SearchTerm()
            {
                Term = x.Key,
                PictureIds = x.Value
            }).ToList();

            return result;
        }

        private List<SearchTerm> GetTerms(string value, string id, char separator = ' ')
        {
            if (!string.IsNullOrEmpty(value) ||
                !string.IsNullOrWhiteSpace(value))
            {
                return value.Split(separator).Select(x => new SearchTerm()
                {
                    PictureIds = new List<string> { id },
                    Term = x
                }).ToList();
            }

            return new List<SearchTerm>();
        }

        private List<SearchTerm> CombineSearchTerms(params List<SearchTerm>[] listOfSearchTerms)
        {
            var all = listOfSearchTerms.SelectMany(x => x).ToList();
            var result = this.ReduceTerms(all);

            return result;
        }
    }
}