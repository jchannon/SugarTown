using AgnosticPaging;

namespace SugarTownDemoMVC.Infrastructure
{
    using System;
    using System.Web;
    using System.Web.Mvc;

    public static class HtmlExtensions
    {
        public static IHtmlString PagedListPager<T>(this HtmlHelper html, Paged<T> model, Func<int, string> generatePageUrl)
        {
            return MvcHtmlString.Create(AgnosticPaging.HtmlHelper.PagedListPager(model,generatePageUrl));
        }
    }
}
