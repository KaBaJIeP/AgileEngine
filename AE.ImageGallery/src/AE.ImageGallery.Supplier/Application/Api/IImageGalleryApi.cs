using System.Threading.Tasks;
using AE.ImageGallery.Supplier.Application.Contracts;
using RestEase;

namespace AE.ImageGallery.Supplier.Application.Api
{
    public interface IImageGalleryApi
    {
        [Post("auth")]
        Task<AuthResponseDto> Auth([Body] AuthRequestDto request);

        [Get("images")]
        Task<PicturePageResponseDto> GetImages([Header("Authorization")] string auth);

        [Get("images")]
        Task<PicturePageResponseDto> GetImages([Header("Authorization")] string auth, [Query] int page);

        [Get("images/{id}")]
        Task<PictureResponseDto> GetImage([Header("Authorization")] string auth, [Path] string id);
    }
}