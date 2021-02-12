using System.Collections.Generic;

namespace AE.ImageGallery.Supplier.Application.Api
{
    public interface ISearchTermService
    {
        public List<SearchTerm> CombineSearchTerms(params List<SearchTerm>[] listOfSearchTerms);
        public List<SearchTerm> GetSearchTerms(ImagesOnPage imagesOnPage);
    }
}