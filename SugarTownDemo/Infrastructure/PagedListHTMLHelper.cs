using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nancy.ViewEngines.Razor;
using SugarTown.Infrastructure;

namespace SugarTownDemo.Infrastructure
{
    ///<summary>
    ///	Extension methods for generating paging controls that can operate on instances of Paged.
    ///</summary>
    public static class HtmlHelper
    {
        private static string WrapInListItem(string text)
        {
            string li = "<li>" + text + "</li>";
            return li;
        }

        private static string WrapInListItem(string inner, PagedListRenderOptions options, params string[] classes)
        {
            string li = "<li";
            int counter = 0;
            foreach (var @class in classes)
            {
                if (counter == 0)
                {
                    li += " class='" + @class + " ";
                }
                else
                {
                    li += @class + " ";

                }
                counter++;
            }

            if (counter > 0)
            {
                li = li.Trim() + "'>";
            }
            else
            {
                li = li.Trim() + ">";
            }


            li += inner;
            li += "</li>";
            return li;
        }

        private static string First<T>(Paged<T> list, Func<int, string> generatePageUrl, PagedListRenderOptions options)
        {
            const int targetPageNumber = 1;
            var first = "<a>";

            string InnerHtml = string.Format(options.LinkToFirstPageFormat, targetPageNumber);

            first += InnerHtml;
            first += "</a>";

            if (list.IsFirstPage)
                return WrapInListItem(first, options, "Paged-skipToFirst", "disabled");

            string link = " href='" + generatePageUrl(targetPageNumber) + "'";
            first = first.Insert(2, link);

            return WrapInListItem(first, options, "Paged-skipToFirst");
        }

        private static string Previous<T>(Paged<T> list, Func<int, string> generatePageUrl, PagedListRenderOptions options)
        {
            var targetPageNumber = list.PageIndex-1;
            var previous = "<a>";

            string InnerHtml = string.Format(options.LinkToPreviousPageFormat, targetPageNumber);

            previous += InnerHtml;
            previous += "</a>";

            if (!list.IsPreviousPage)
                return WrapInListItem(previous, options, "Paged-skipToPrevious", "disabled");

            string link = " href='" + generatePageUrl(targetPageNumber) + "'";

            previous = previous.Insert(2, link);

            return WrapInListItem(previous, options, "Paged-skipToPrevious");
        }

        private static string Page<T>(int i, Paged<T> list, Func<int, string> generatePageUrl, PagedListRenderOptions options)
        {
            var format = options.FunctionToDisplayEachPageNumber
                ?? (pageNumber => string.Format(options.LinkToIndividualPageFormat, pageNumber));
            var targetPageNumber = i;
            var page = "<a>";

            string InnerHtml = format(targetPageNumber);

            page += InnerHtml;
            page += "</a>";

            if (i == list.PageIndex)
                return WrapInListItem(page, options, "active");

            string link = " href='" + generatePageUrl(targetPageNumber) + "'";
            page = page.Insert(2, link);

            return WrapInListItem(page, options);
        }

        private static string Next<T>(Paged<T> list, Func<int, string> generatePageUrl, PagedListRenderOptions options)
        {
            var targetPageNumber = list.PageIndex + 1;

            var next = "<a>";

            string InnerHtml = string.Format(options.LinkToNextPageFormat, targetPageNumber);
            next += InnerHtml;
            next += "</a>";

            if (!list.IsNextPage)
                return WrapInListItem(next, options, "Paged-skipToNext", "disabled");

            string link = " href='" + generatePageUrl(targetPageNumber) + "'";
            next = next.Insert(2, link);

            return WrapInListItem(next, options, "Paged-skipToNext");
        }

        private static string Last<T>(Paged<T> list, Func<int, string> generatePageUrl, PagedListRenderOptions options)
        {
            var targetPageNumber = list.PageCount;
            var last = "<a>";
            string InnerHtml = string.Format(options.LinkToLastPageFormat, targetPageNumber);
            last += InnerHtml;
            last += "</a>";

            if (list.IsLastPage)
                return WrapInListItem(last, options, "Paged-skipToLast", "disabled");

            string link = " href='" + generatePageUrl(targetPageNumber) + "'";
            last = last.Insert(2, link);

            return WrapInListItem(last, options, "Paged-skipToLast");
        }

        private static string PageCountAndLocationText<T>(Paged<T> list, PagedListRenderOptions options)
        {
            var text = "<a>";
            string inner = string.Format(options.PageCountAndCurrentLocationFormat, list.PageIndex, list.PageCount);
            text += inner + "</a>";

            return WrapInListItem(text, options, "Paged-pageCountAndLocation", "disabled");
        }

        private static string ItemSliceAndTotalText<T>(Paged<T> list, PagedListRenderOptions options)
        {
            var text = "<a>";
            string inner = string.Format(options.ItemSliceAndTotalFormat, list.FirstItemOnPage, list.LastItemOnPage, list.TotalCount);
            text += inner + "</a>";

            return WrapInListItem(text, options, "Paged-pageCountAndLocation", "disabled");
        }

        private static string Ellipses(PagedListRenderOptions options)
        {
            var a = "<a>";

            string InnerHtml = options.EllipsesFormat;
            a += InnerHtml + "</a>";

            return WrapInListItem(a, options, "Paged-ellipses", "disabled");
        }

        ///<summary>
        ///	Displays a configurable paging control for instances of Paged.
        ///</summary>
        ///<param name = "html">This method is meant to hook off HtmlHelper as an extension method.</param>
        ///<param name = "list">The Paged to use as the data source.</param>
        ///<param name = "generatePageUrl">A function that takes the page number of the desired page and returns a URL-string that will load that page.</param>
        ///<returns>Outputs the paging control HTML.</returns>
        public static IHtmlString PagedListPager<T, U>(this HtmlHelpers<T> html,
                                                   Paged<U> list,
                                                   Func<int, string> generatePageUrl)
        {
            return PagedListPager(html, list, generatePageUrl, new PagedListRenderOptions());
        }

        ///<summary>
        ///	Displays a configurable paging control for instances of Paged.
        ///</summary>
        ///<param name = "html">This method is meant to hook off HtmlHelper as an extension method.</param>
        ///<param name = "list">The Paged to use as the data source.</param>
        ///<param name = "generatePageUrl">A function that takes the page number  of the desired page and returns a URL-string that will load that page.</param>
        ///<param name = "options">Formatting options.</param>
        ///<returns>Outputs the paging control HTML.</returns>
        public static IHtmlString PagedListPager<T, U>(this HtmlHelpers<T> html,
                                                   Paged<U> list,
                                                   Func<int, string> generatePageUrl,
                                                   PagedListRenderOptions options)
        {
            var listItemLinks = new List<string>();

            //first
            if (options.DisplayLinkToFirstPage)
                listItemLinks.Add(First(list, generatePageUrl, options));

            //previous
            if (options.DisplayLinkToPreviousPage)
                listItemLinks.Add(Previous(list, generatePageUrl, options));

            //text
            if (options.DisplayPageCountAndCurrentLocation)
                listItemLinks.Add(PageCountAndLocationText(list, options));

            //text
            if (options.DisplayItemSliceAndTotal)
                listItemLinks.Add(ItemSliceAndTotalText(list, options));

            //page
            if (options.DisplayLinkToIndividualPages)
            {
                //calculate start and end of range of page numbers
                var start = 1;
                var end = list.PageCount;
                if (options.MaximumPageNumbersToDisplay.HasValue && list.PageCount > options.MaximumPageNumbersToDisplay)
                {
                    var maxPageNumbersToDisplay = options.MaximumPageNumbersToDisplay.Value;
                    start = list.PageIndex - maxPageNumbersToDisplay / 2;
                    if (start < 1)
                        start = 1;
                    end = maxPageNumbersToDisplay;
                    if ((start + end - 1) > list.PageCount)
                        start = list.PageCount - maxPageNumbersToDisplay + 1;
                }

                //if there are previous page numbers not displayed, show an ellipsis
                if (options.DisplayEllipsesWhenNotShowingAllPageNumbers && start > 1)
                    listItemLinks.Add(Ellipses(options));

                foreach (var i in Enumerable.Range(start, end))
                {
                    //show delimiter between page numbers
                    if (i > start && !string.IsNullOrWhiteSpace(options.DelimiterBetweenPageNumbers))
                        listItemLinks.Add(WrapInListItem(options.DelimiterBetweenPageNumbers));

                    //show page number link
                    listItemLinks.Add(Page(i, list, generatePageUrl, options));
                }

                //if there are subsequent page numbers not displayed, show an ellipsis
                if (options.DisplayEllipsesWhenNotShowingAllPageNumbers && (start + end - 1) < list.PageCount)
                    listItemLinks.Add(Ellipses(options));
            }

            //next
            if (options.DisplayLinkToNextPage)
                listItemLinks.Add(Next(list, generatePageUrl, options));

            //last
            if (options.DisplayLinkToLastPage)
                listItemLinks.Add(Last(list, generatePageUrl, options));

            //append class to first item in list?
            if (!string.IsNullOrWhiteSpace(options.ClassToApplyToFirstListItemInPager))
            {
                var firstItem = listItemLinks.First();

                if (firstItem.Contains("class"))
                {
                    string classcontents = firstItem.Substring(0, firstItem.IndexOf("<a"));
                    string classcss = options.ClassToApplyToFirstListItemInPager + " ";

                    int closingBracket = firstItem.IndexOf("'>") + 2;
                    string afterClosingBracket = firstItem.Substring(closingBracket);


                    classcontents = classcontents.Insert(11, classcss);

                    firstItem = classcontents + afterClosingBracket;
                    listItemLinks[0] = firstItem;
                }
                else
                {
                    string classcss = " class='" + options.ClassToApplyToFirstListItemInPager + "'";

                    firstItem = firstItem.Insert(3, classcss);
                    listItemLinks[0] = firstItem;
                }

            }

            //append class to last item in list?
            if (!string.IsNullOrWhiteSpace(options.ClassToApplyToLastListItemInPager))
            {
                var lastItem = listItemLinks.Last();

                if (lastItem.Contains("class"))
                {
                    string classcontents = lastItem.Substring(0, lastItem.IndexOf("<a"));
                    string classcss = options.ClassToApplyToLastListItemInPager + " ";

                    int closingBracket = lastItem.IndexOf("'>") + 2;
                    string afterClosingBracket = lastItem.Substring(closingBracket);


                    classcontents = classcontents.Insert(11, classcss);

                    lastItem = classcontents + afterClosingBracket;
                    listItemLinks[listItemLinks.Count - 1] = lastItem;
                }
                else
                {
                    string classcss = " class='" + options.ClassToApplyToLastListItemInPager + "'";

                    lastItem = lastItem.Insert(3, classcss);
                    listItemLinks[listItemLinks.Count - 1] = lastItem;
                }
            }

            int counter = 0;
            //append classes to all list item links
            foreach (var li in listItemLinks)
            {
                foreach (var c in options.LiElementClasses ?? Enumerable.Empty<string>())
                {
                    if (li.Contains("class"))
                    {
                        string classcontents = li.Substring(li.IndexOf("class"), li.IndexOf("'>"));
                        string classcss = c;
                        classcontents.Insert(7, classcss);
                    }
                    else
                    {
                        string classcss = " class='" + c + "'";

                        li.Insert(3, classcss);
                    }
                }
            }

            //collapse all of the list items into one big string
            var listItemLinksString = listItemLinks.Aggregate(
                new StringBuilder(),
                (sb, listItem) => sb.Append(listItem.ToString()),
                sb => sb.ToString()
                );

            var ul = "<ul";

            counter = 0;

            foreach (var c in options.UlElementClasses ?? Enumerable.Empty<string>())
            {
                if (counter == 0)
                {
                    ul += " class='" + c + " ";
                }
                else
                {
                    ul += c + " ";
                }
                counter++;
            }

            if (counter > 0)
            {
                ul = ul.Trim() + "'>";
            }
            else
            {
                ul = ul.Trim() + ">";
            }

            string InnerHtml = listItemLinksString;
            ul += InnerHtml;
            ul += "</ul>";


            var outerDiv = "<div";

            counter = 0;
            foreach (var c in options.ContainerDivClasses ?? Enumerable.Empty<string>())
            {
                if (counter == 0)
                {
                    outerDiv += " class='" + c + " ";
                }
                else
                {
                    outerDiv += c + " ";
                }
                counter++;
            }

            if (counter > 0)
            {
                outerDiv = outerDiv.Trim() + "'>";
            }
            else
            {
                outerDiv = outerDiv.Trim() + ">";
            }

            outerDiv += ul;

            outerDiv += "</div>";

            return new NonEncodedHtmlString(outerDiv);
        }

        /////<summary>
        ///// Displays a configurable "Go To Page:" form for instances of Paged.
        /////</summary>
        /////<param name="html">This method is meant to hook off HtmlHelper as an extension method.</param>
        /////<param name="list">The Paged to use as the data source.</param>
        /////<param name="formAction">The URL this form should submit the GET request to.</param>
        /////<returns>Outputs the "Go To Page:" form HTML.</returns>
        //public static IHtmlString PagedListGoToPageForm(this System.Web.Mvc.HtmlHelper html,
        //                                                  Paged list,
        //                                                  string formAction)
        //{
        //    return PagedListGoToPageForm(html, list, formAction, "page");
        //}

        /////<summary>
        ///// Displays a configurable "Go To Page:" form for instances of Paged.
        /////</summary>
        /////<param name="html">This method is meant to hook off HtmlHelper as an extension method.</param>
        /////<param name="list">The Paged to use as the data source.</param>
        /////<param name="formAction">The URL this form should submit the GET request to.</param>
        /////<param name="inputFieldName">The querystring key this form should submit the new page number as.</param>
        /////<returns>Outputs the "Go To Page:" form HTML.</returns>
        //public static IHtmlString PagedListGoToPageForm(this System.Web.Mvc.HtmlHelper html,
        //                                                  Paged<> list,
        //                                                  string formAction,
        //                                                  string inputFieldName)
        //{
        //    return PagedListGoToPageForm(html, list, formAction, new GoToFormRenderOptions(inputFieldName));
        //}

        /////<summary>
        ///// Displays a configurable "Go To Page:" form for instances of Paged.
        /////</summary>
        /////<param name="html">This method is meant to hook off HtmlHelper as an extension method.</param>
        /////<param name="list">The Paged to use as the data source.</param>
        /////<param name="formAction">The URL this form should submit the GET request to.</param>
        /////<param name="options">Formatting options.</param>
        /////<returns>Outputs the "Go To Page:" form HTML.</returns>
        //public static IHtmlString PagedListGoToPageForm(this System.Web.Mvc.HtmlHelper html,
        //                                         Paged list,
        //                                         string formAction,
        //                                         GoToFormRenderOptions options)
        //{
        //    var form = new TagBuilder("form");
        //    form.AddCssClass("Paged-goToPage");
        //    form.Attributes.Add("action", formAction);
        //    form.Attributes.Add("method", "get");

        //    var fieldset = new TagBuilder("fieldset");

        //    var label = new TagBuilder("label");
        //    label.Attributes.Add("for", options.InputFieldName);
        //    label.SetInnerText(options.LabelFormat);

        //    var input = new TagBuilder("input");
        //    input.Attributes.Add("type", options.InputFieldType);
        //    input.Attributes.Add("name", options.InputFieldName);
        //    input.Attributes.Add("value", list.PageNumber.ToString());

        //    var submit = new TagBuilder("input");
        //    submit.Attributes.Add("type", "submit");
        //    submit.Attributes.Add("value", options.SubmitButtonFormat);

        //    fieldset.InnerHtml = label.ToString();
        //    fieldset.InnerHtml += input.ToString(TagRenderMode.SelfClosing);
        //    fieldset.InnerHtml += submit.ToString(TagRenderMode.SelfClosing);
        //    form.InnerHtml = fieldset.ToString();
        //    return new IHtmlString(form.ToString());
        //}
    }
}