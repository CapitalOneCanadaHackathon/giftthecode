using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Indspire.Soaring.Engagement.Models
{
    public class Pagination
    {
        public int TotalCount { get; set; }

        public int PageSize { get; set; }

        public int Page { get; set; }

        public string Search { get; set; }

        public string BaseUrl { get; set; } = string.Empty;

        public string QueryStringToken { get; set; } = "?";

        public string QueryStringParam { get; set; } = "page";

        public string QueryStringPageSizeParam { get; set; } = "pageSize";

        public string QueryStringSearchParam { get; set; } = "search";
    }
}
