using System.Threading.Tasks;

namespace AE.ImageGallery.Supplier.Application.Api
{
    public interface IImageRepository
    {
        Task Save(ImagesOnPage imagesOnPage);
        Task DeleteAllImages();
    }
}