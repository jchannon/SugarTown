using System;
using AgnosticPaging;
using Nancy.ViewEngines.Razor;
using SugarTown.Models;

namespace SugarTownDemo.Infrastructure
{
    public static class HtmlExtensions
    {
        public static IHtmlString PagedListPager<T>(this HtmlHelpers<T> html, Paged<Post> model, Func<int, string> generatePageUrl)
        {
            return new NonEncodedHtmlString(AgnosticPaging.HtmlHelper.PagedListPager(model, generatePageUrl));
        }
    }
}