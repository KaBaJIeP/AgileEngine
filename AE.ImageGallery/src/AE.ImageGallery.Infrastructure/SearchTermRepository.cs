using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AE.ImageGallery.Application.Api;
using Microsoft.Extensions.Caching.Distributed;

namespace AE.ImageGallery.Infrastructure
{
    public class SearchTermRepository : ISearchTermRepository
    {
        private readonly IDistributedCache _cache;
        private const string Separator = ",";

        public SearchTermRepository(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<List<string>> GetImageIds(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm) || string.IsNullOrWhiteSpace(searchTerm))
                return new List<string>();

            var idsBytes = await _cache.GetAsync(searchTerm.ToLowerInvariant());
            if (idsBytes == null || idsBytes.Length == 0)
                return new List<string>();

            var idsString = Encoding.ASCII.GetString(idsBytes);
            var ids = idsString.Split(Separator).ToList();

            return ids;
        }
    }
}