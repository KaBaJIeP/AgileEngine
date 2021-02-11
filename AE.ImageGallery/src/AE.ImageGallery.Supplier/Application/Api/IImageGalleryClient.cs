using System.Threading.Tasks;
using AE.ImageGallery.Supplier.Application.Contracts;

namespace AE.ImageGallery.Supplier.Application.Api
{
    public interface IImageGalleryClient
    {
        Task<AuthResponseDto> Auth(string apiKey);
        Task<PicturePageResponseDto> GetImages();
        Task<PicturePageResponseDto> GetImages(int page);
        Task<PictureResponseDto> GetImage(string id);
    }
}