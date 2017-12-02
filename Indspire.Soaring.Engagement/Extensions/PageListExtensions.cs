namespace Indspire.Soaring.Engagement.Extensions
{
    using Indspire.Soaring.Engagement.Models;

    public static class PageListExtensions
    {
        public static Pagination GetPagination<TList>(this PagedList<TList> pagedList)
        {
            var pagination = new Pagination();

            if (pagedList != null)
            {
                pagination = new Pagination()
                {
                    TotalCount = pagedList.TotalCount,

                    PageSize = pagedList.PageSize,

                    Page = pagedList.Page
                };
            }

            return pagination;
        }
    }
}
