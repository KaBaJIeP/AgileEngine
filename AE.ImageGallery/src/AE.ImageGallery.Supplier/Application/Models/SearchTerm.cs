using System;
using System.Collections.Generic;
using System.Linq;

namespace AE.ImageGallery.Supplier.Application
{
    public class SearchTerm
    {
        public List<string> ImageIds { get; set; }
        public string Term { get; set; }
    }

    public class SearchTermComparer : IEqualityComparer<SearchTerm>
    {
        public bool Equals(SearchTerm x, SearchTerm y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return x.ImageIds.Count == y.ImageIds.Count
                   && x.ImageIds.All(id => y.ImageIds.Contains(id))
                   && String.Equals(x.Term, y.Term, StringComparison.InvariantCultureIgnoreCase);
        }

        public int GetHashCode(SearchTerm obj)
        {
            return HashCode.Combine(obj.ImageIds, obj.Term);
        }
    }
}