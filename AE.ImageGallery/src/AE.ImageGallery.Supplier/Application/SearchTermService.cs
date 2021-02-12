using System.Collections.Generic;
using System.Linq;
using AE.ImageGallery.Supplier.Application.Api;
using AE.ImageGallery.Supplier.Application.Contracts;

namespace AE.ImageGallery.Supplier.Application
{
    public class SearchTermService : ISearchTermService
    {
        private readonly IEqualityComparer<SearchTerm> _comparer;

        public SearchTermService(IEqualityComparer<SearchTerm> comparer)
        {
            _comparer = comparer;
        }

        public SearchTermsOnPage GetSearchTermsOnPage(ImagesOnPage imagesOnPage)
        {
            var imagesSearchTerms = imagesOnPage.Images.Select(this.MapToTerms)
                .SelectMany(x => x)
                .ToList();
            var searchTerms = this.ReduceTerms(imagesSearchTerms);

            var result = new SearchTermsOnPage
            {
                SearchTerms = searchTerms,
                ImagesOnPage = imagesOnPage
            };

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
                    y => y.Select(z => z.ImageIds)
                        .SelectMany(ids => ids).Distinct().ToList());
            var result = map.Select(x => new SearchTerm()
            {
                Term = x.Key,
                ImageIds = x.Value
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
                    ImageIds = new List<string> { id },
                    Term = x
                }).ToList();
            }

            return new List<SearchTerm>();
        }


    }
}