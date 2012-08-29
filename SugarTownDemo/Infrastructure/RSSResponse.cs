using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Web;
using System.Xml;
using Nancy;
using SugarTown.Models;

namespace SugarTownDemo.Infrastructure
{
    public static class FormatterExtensions
    {
        public static Response AsRSS(this IResponseFormatter formatter, IEnumerable<Post> model, string RSSTitle)
        {
            return new RSSResponse(model, RSSTitle, formatter.Context.Request.Url);
        }
    }

    public class RSSResponse : Response
    {
        private string RSSTitle { get; set; }
        private Uri URL { get; set; }

        public RSSResponse(IEnumerable<Post> model, string RSSTitle, Uri URL)
        {
            this.RSSTitle = RSSTitle; ;
            this.URL = URL;

            this.Contents = GetXmlContents(model);
            this.ContentType = "application/rss+xml";
            this.StatusCode = HttpStatusCode.OK;
        }

        private Action<Stream> GetXmlContents(IEnumerable<Post> model)
        {
            var items = new List<SyndicationItem>();

            foreach (Post post in model)
            {
                string contentString = String.Format("{0}  {1:MMM dd, yyyy}  {2}",
                            post.Title, post.DateCreated, post.Body);

                var item = new SyndicationItem(
                    title: post.Title,
                    content: contentString,
                    itemAlternateLink: new Uri("http://mydomain.com/blog/" + post.Slug),
                    id: "http://mydomain.com/blog/" + post.Slug,
                    lastUpdatedTime: post.DateCreated.ToUniversalTime()
                    );
                item.PublishDate = post.DateCreated.ToUniversalTime();
                item.Summary = new TextSyndicationContent(contentString, TextSyndicationContentKind.Plaintext);
                items.Add(item);
            }

            SyndicationFeed feed = new SyndicationFeed(
                this.RSSTitle,
                this.RSSTitle, /* Using Title also as Description */
                this.URL,
                items);

            Rss20FeedFormatter formatter = new Rss20FeedFormatter(feed);

            return stream =>
            {
                using (XmlWriter writer = XmlWriter.Create(stream))
                {
                    formatter.WriteTo(writer);

                }
            };
        }
    }
}