using System.Collections.Generic;
using System.Threading.Tasks;

namespace AE.ImageGallery.Application.Api
{
    public interface ISearchTermRepository
    {
        Task<List<string>> GetImageIds(string searchTerm);
    }
}