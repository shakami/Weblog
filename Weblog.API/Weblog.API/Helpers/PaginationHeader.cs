using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Weblog.API.Helpers
{
    public static class PaginationHeader<T>
    {
        public static KeyValuePair<string, StringValues> Get(PagedList<T> entities)
        {
            var paginationMetadata = new
            {
                totalCount = entities.TotalCount,
                pageSize = entities.PageSize,
                currentPage = entities.CurrentPage,
                totalPages = entities.TotalPages,
            };

            return new KeyValuePair<string, StringValues>("X-Pagination",
                JsonSerializer.Serialize(paginationMetadata));
        }
    }
}
