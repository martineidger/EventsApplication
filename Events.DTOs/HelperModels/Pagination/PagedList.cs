using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.DTOs.HelperModels.Pagination
{
    public class PagedList<T> : List<T>
    {
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public PagedList(List<T> items,int count, int pageNumber, int pageSize)
        {
            CurrentPage = pageNumber;
            TotalCount = count;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);

            AddRange(items);
        }

        public static PagedList<T> Paginate(List<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count;
            var skipCount = (pageNumber - 1) * pageSize;

            var paginatedItems = source.Skip(skipCount).Take(pageSize).ToList();

            return new PagedList<T>(paginatedItems, count, pageNumber, pageSize);
        }
    }
}
