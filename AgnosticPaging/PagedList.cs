using System;
using System.Collections.Generic;
using System.Linq;

namespace AgnosticPaging
{
    public class Paged<T>
    {
        public Paged()
        {

        }

        public Paged(IEnumerable<T> source, int index, int totalResults, int pageSize)
        {
            this.TotalCount = totalResults;
            this.PageSize = pageSize;
            this.PageIndex = index;
            this.Data = new List<T>(source);
        }

        public Paged(IEnumerable<T> source, int index, int totalResults)
        {
            this.TotalCount = totalResults;
            this.PageSize = 25;
            this.PageIndex = index;
            this.Data = new List<T>(source);
        }

        public List<T> Data { get; set; }
        public int TotalCount { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        public int PageCount
        {
            get
            {
                return
                    TotalCount > 0
                        ? (int)Math.Ceiling(TotalCount / (double)PageSize)
                        : 0;
            }
        }

        public int FirstItemOnPage
        {
            get { return (PageIndex) * PageSize + 1; }
        }



        public int LastItemOnPage
        {
            get
            {
                int numberOfLastItemOnPage = FirstItemOnPage + PageSize - 1;
                return  (numberOfLastItemOnPage > TotalCount
                             ? TotalCount
                             : numberOfLastItemOnPage);
            }
        }


        public bool IsPreviousPage
        {
            get
            {
                return (PageIndex > 1);
            }
        }

        public bool IsNextPage
        {
            get
            {
                return (PageIndex * PageSize) <= TotalCount;
            }
        }

        public bool IsFirstPage
        {
            get { return PageIndex == 1; }
        }

        public bool IsLastPage
        {
            get { return PageIndex >= PageCount; }
        }
    }

    public static class Pagination
    {
        //public static PagedList<T> ToPagedList<T>(this IEnumerable<T> source, int index)
        //{
        //    return new PagedList<T>(source, index, 25);
        //}

        //public static PagedList<T> ToPagedList<T>(this IEnumerable<T> source, int index, int pageSize)
        //{
        //    return new PagedList<T>(source, index, pageSize);
        //}

        public static Paged<T> ToPaged<T>(this IEnumerable<T> source, int index, int totalResults)
        {
            return new Paged<T>(source, index, totalResults);
        }

        //public static Paged<T> ToPaged<T>(this IEnumerable<T> source, int index)
        //{
        //    return new Paged<T>(source, index, 25);
        //}
    }
}