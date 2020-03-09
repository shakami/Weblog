using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Weblog.API.ResourceParameters
{
    public class BlogsResourceParameters
    {
        public string SearchQuery { get; set; }
        public int PageNumber { get; set; } = 1;

        const int maxPageSize = 25;
        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > maxPageSize) ? maxPageSize : value;
        }
    }
}
