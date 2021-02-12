using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AE.ImageGallery.Supplier.Application.Api;
using AE.ImageGallery.Supplier.Application.Contracts;

namespace AE.ImageGallery.Supplier.Application
{
    public class ImageGalleryService : IImageGalleryService
    {
        private readonly IImageGalleryApiClient _apiClient;

        public ImageGalleryService(IImageGalleryApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<ImagesOnPage> GetImagesOnPage(int pageNumber)
        {
            var page = await _apiClient.GetImages(pageNumber);
            var imageIds = page.Pictures.Select(x => x.Id)
                .Where(x => !string.IsNullOrEmpty(x))
                .Distinct()
                .ToList();
            var getImageTasks = new List<Task<PictureResponseDto>>();

            foreach (var imageId in imageIds)
            {
                var getImageTask = _apiClient.GetImage(imageId);
                getImageTasks.Add(getImageTask);
            }
            var pictures = (await Task.WhenAll(getImageTasks)).ToList();

            return new ImagesOnPage
            {
                Pictures = pictures,
                TotalPagesCount = page.PageCount,
                CurrentPage = pageNumber,
                HasMore = page.HasMore
            };
        }
    }
}