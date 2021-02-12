using System.Threading.Tasks;

namespace AE.ImageGallery.Supplier.Application.Api
{
    public interface IImageGalleryService
    {
        public Task<ImagesOnPage> GetImagesOnPage(int pageNumber);
    }
}