using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Indspire.Soaring.Engagement.Models
{
    public class PagedList<TList> : IEnumerable<TList>
    {
        public int TotalCount { get; set; }

        public int Page { get; set; }

        public int PageSize { get; set; }

        public string Search { get; set; }

        public IEnumerable<TList> List { get; set; }

        public PagedList(IEnumerable<TList> list)
        {
            if (list == null)
            {
                this.List = new List<TList>();
            }
            else
            {
                this.List = list;
            }

            this.TotalCount = 0;
            this.Page = 1;
            this.PageSize = 10;
            this.Search = null;
        }

        public IEnumerator<TList> GetEnumerator()
        {
            return List.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
