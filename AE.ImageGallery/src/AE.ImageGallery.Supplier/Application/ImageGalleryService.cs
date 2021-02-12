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

        public ImageGalleryService(IImageGalleryClient client)
        {
            _client = client;
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
    }
}