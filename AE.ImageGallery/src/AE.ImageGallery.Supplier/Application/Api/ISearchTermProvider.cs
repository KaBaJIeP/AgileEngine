using System.Collections.Generic;
using System.Threading.Tasks;

namespace AE.ImageGallery.Supplier.Application.Api
{
    public interface ISearchTermProvider
    {
        Task<List<SearchTerm>> GetSearchTerms();
    }
}