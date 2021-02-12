using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AE.ImageGallery.Supplier.Application.Api;
using AE.ImageGallery.Supplier.Application.Contracts;

namespace AE.ImageGallery.Supplier.Application
{
    public class ImageGalleryService : IImageGalleryService
    {
        private readonly IImageGalleryClient _client;
        private readonly IEqualityComparer<SearchTerm> _comparer;

        public ImageGalleryService(IImageGalleryClient client, IEqualityComparer<SearchTerm> comparer)
        {
            _client = client;
            _comparer = comparer;
        }

        public async Task<ImagesOnPage> GetImagesOnPage(int pageNumber)
        {
            var page = await _client.GetImages(pageNumber);
            var imageIds = page.Pictures.Select(x => x.Id)
                .Where(x => !string.IsNullOrEmpty(x))
                .Distinct()
                .ToList();
            var getImageTasks = new List<Task<PictureResponseDto>>();

            foreach (var imageId in imageIds)
            {
                var getImageTask = _client.GetImage(imageId);
                getImageTasks.Add(getImageTask);
            }
            var pictures = (await Task.WhenAll(getImageTasks)).ToList();

            return new ImagesOnPage
            {
                Pictures = pictures,
                PageCount = page.PageCount
            };
        }

        public List<SearchTerm> MapToTerms(PictureResponseDto picture)
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

        public List<SearchTerm> ReduceToTerms(List<SearchTerm> searchTerms)
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