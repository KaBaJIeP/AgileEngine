using System.Threading.Tasks;

namespace AE.ImageGallery.Supplier.Application.Api
{
    public interface ISearchTermProvider
    {
        Task<SearchTermsOnPage> GetSearchTermsOnPage(int pageNumber);
    }
}