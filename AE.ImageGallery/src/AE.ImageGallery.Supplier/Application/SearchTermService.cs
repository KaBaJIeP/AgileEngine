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

        public List<SearchTerm> CombineSearchTerms(params List<SearchTerm>[] listOfSearchTerms)
        {
            var all = listOfSearchTerms.SelectMany(x => x).ToList();
            var result = this.ReduceTerms(all);

            return result;
        }

        public List<SearchTerm> GetSearchTerms(ImagesOnPage imagesOnPage)
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


    }
}