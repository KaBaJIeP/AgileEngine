namespace AE.ImageGallery.Supplier.Application.Api
{
    public interface ISearchTermService
    {
        public SearchTermsOnPage GetSearchTermsOnPage(ImagesOnPage imagesOnPage);
    }
}