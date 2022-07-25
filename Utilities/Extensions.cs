using System.Collections.Generic;
using System.Linq;

namespace Utilities
{
    public static class Extensions
    {
        public static IEnumerable<T> GetPage<T>(this IEnumerable<T> list, int pageNumber, int itemsPerPage = 5)
        {
            return list.Skip((pageNumber - 1) * itemsPerPage).Take(itemsPerPage);
        }
    }
}