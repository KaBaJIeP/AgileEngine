using System.Collections.Generic;
using System.Threading.Tasks;

namespace AE.ImageGallery.Supplier.Application.Api
{
    public interface ISearchTermsRepository
    {
        Task Save(List<SearchTerm> terms);
    }
}