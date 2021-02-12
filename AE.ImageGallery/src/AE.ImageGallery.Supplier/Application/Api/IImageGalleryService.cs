using System.Collections.Generic;
using System.Threading.Tasks;

namespace AE.ImageGallery.Supplier.Application.Api
{
    public interface IImageGalleryService
    {
        public Task<ImagesOnPage> GetImagesOnPage(int pageNumber);
        public List<SearchTerm> GetSearchTerms(ImagesOnPage imagesOnPage);

        // api not for production -> just for nice api ^^
        public List<SearchTerm> CombineSearchTerms(params List<SearchTerm>[] listOfSearchTerms);
    }
}