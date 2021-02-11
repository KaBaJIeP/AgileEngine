using System.Threading.Tasks;
using AE.ImageGallery.Supplier.Application.Api;
using AE.ImageGallery.Supplier.Application.Contracts;

namespace AE.ImageGallery.Supplier.Application
{
    public class ImageGalleryClient : IImageGalleryClient
    {
        private readonly IImageGalleryApi _imageGalleryApi;

        public ImageGalleryClient(IImageGalleryApi imageGalleryApi)
        {
            _imageGalleryApi = imageGalleryApi;
        }

        public async Task<AuthResponseDto> Auth(string apiKey)
        {
            if (string.IsNullOrEmpty(apiKey))
                return new AuthResponseDto
                {
                    Auth = false,
                    Token = null
                };

            var request = new AuthRequestDto
            {
                ApiKey = apiKey
            };
            return await _imageGalleryApi.Auth(request);
        }

        public Task<PicturePageResponseDto> GetImages()
        {
            return _imageGalleryApi.GetImages();
        }

        public Task<PicturePageResponseDto> GetImages(int page)
        {
            return _imageGalleryApi.GetImages(page);
        }

        public Task<PictureResponseDto> GetImage(string id)
        {
            return _imageGalleryApi.GetImage(id);
        }
    }
}