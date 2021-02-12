using System;
using System.Net;
using System.Threading.Tasks;
using AE.ImageGallery.Supplier.Application.Api;
using AE.ImageGallery.Supplier.Application.Contracts;
using AE.ImageGallery.Supplier.Configs;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;
using RestEase;

namespace AE.ImageGallery.Supplier.Application
{
    public class ImageGalleryApiClient : IImageGalleryApiClient
    {
        private readonly IImageGalleryApi _imageGalleryApi;
        private readonly IOptions<AgileEngineConfig> _config;
        private readonly AsyncRetryPolicy _unauthorizedPolicy;
        private string _token;

        public ImageGalleryApiClient(IImageGalleryApi imageGalleryApi, IOptions<AgileEngineConfig> config)
        {
            _imageGalleryApi = imageGalleryApi;
            _config = config;
            _unauthorizedPolicy = Policy
                .Handle<ApiException>(exception => exception.StatusCode == HttpStatusCode.Unauthorized)
                .RetryAsync(OnRetryAsync);
        }

        private async Task OnRetryAsync(Exception ex, int retryCount, Context context)
        {
            var response = await this.Auth(_config.Value.ApiKey);
            if (response.Auth)
                _token = $"Bearer {response.Token}";
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
            return _unauthorizedPolicy.ExecuteAsync(() => _imageGalleryApi.GetImages(_token));
        }

        public Task<PicturePageResponseDto> GetImages(int page)
        {
            return _unauthorizedPolicy.ExecuteAsync(() => _imageGalleryApi.GetImages(_token, page));
        }

        public Task<PictureResponseDto> GetImage(string id)
        {
            return _unauthorizedPolicy.ExecuteAsync(() => _imageGalleryApi.GetImage(_token, id));
        }
    }
}