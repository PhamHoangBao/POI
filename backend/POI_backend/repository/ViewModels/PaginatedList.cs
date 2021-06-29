using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace POI.repository.ViewModels
{
    public class PagedList<T> : List<T>
    {
        public int CurrentPageIndex { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public bool HasPrevious { get; private set; }
        public bool HasNext { get; private set; }
        public PagedList()
        {

        }
        public PagedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            HasPrevious = CurrentPageIndex > 1;
            HasNext = CurrentPageIndex < TotalPages;
            AddRange(items);
        }
        public static PagedList<T> ToPagedList(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return new PagedList<T>(items, count, pageIndex, pageSize);
        }
    }
}
