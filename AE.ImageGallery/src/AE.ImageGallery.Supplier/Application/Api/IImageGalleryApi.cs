using System.Threading.Tasks;
using AE.ImageGallery.Supplier.Application.Contracts;
using RestEase;

namespace AE.ImageGallery.Supplier.Application.Api
{
    public interface IImageGalleryApi
    {
        [Header("Authorization")]
        string AuthorizationHeader { get; set; }

        [Post("auth")]
        Task<AuthResponseDto> Auth([Body] AuthRequestDto request);

        [Get("images")]
        Task<PicturePageResponseDto> GetImages();

        [Get("images/{page}")]
        Task<PicturePageResponseDto> GetImages([Path] int page);

        [Get("images/{id}")]
        Task<PictureResponseDto> GetImage([Path] string id);
    }
}