using System.Collections.Generic;
using System.Threading.Tasks;
using AE.ImageGallery.Supplier.Application.Contracts;

namespace AE.ImageGallery.Supplier.Application.Api
{
    public interface IImageGalleryService
    {
        public Task<ImagesOnPage> GetImagesOnPage(int pageNumber);
        public List<SearchTerm> MapToTerms(PictureResponseDto picture);
        public List<SearchTerm> ReduceToTerms(List<SearchTerm> searchTerms);
    }
}