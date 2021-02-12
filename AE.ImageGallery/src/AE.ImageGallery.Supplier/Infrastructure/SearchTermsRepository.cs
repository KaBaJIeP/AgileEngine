using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AE.ImageGallery.Supplier.Application;
using AE.ImageGallery.Supplier.Application.Api;
using Microsoft.Extensions.Caching.Distributed;

namespace AE.ImageGallery.Supplier.Infrastructure
{
    public class SearchTermsRepository : ISearchTermsRepository
    {
        private readonly IDistributedCache _cache;
        private const string Separator = ",";

        public SearchTermsRepository(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task Save(List<SearchTerm> terms)
        {
            foreach (var term in terms)
            {
                var oldIds = await GetImageIds(term.Term.ToLowerInvariant());
                var newIds = term.ImageIds;

                var mergedIds = oldIds.Union(newIds).ToList();

                var ids = string.Join(Separator, mergedIds);
                var bytes = Encoding.ASCII.GetBytes(ids);
                await _cache.SetAsync(term.Term.ToLowerInvariant(), bytes);
            }
        }

        private async Task<List<string>> GetImageIds(string term)
        {
            var idsBytes = await _cache.GetAsync(term);
            if (idsBytes == null || idsBytes.Length == 0)
                return new List<string>();

            var idsString = Encoding.ASCII.GetString(idsBytes);
            var ids = idsString.Split(Separator).ToList();

            return ids;
        }
    }
}