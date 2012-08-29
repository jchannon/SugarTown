using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using SugarTown.Models;

namespace SugarTownMVC.Infrastructure
{
    public class RssActionResult : ActionResult
    {
        public IEnumerable<Post> Model { get; set; }

        public string RSSTitle { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.ContentType = "application/rss+xml";

            var items = new List<SyndicationItem>();

            foreach (Post post in Model)
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
                context.RequestContext.HttpContext.Request.Url,
                items);

            Rss20FeedFormatter formatter = new Rss20FeedFormatter(feed);


            using (XmlWriter writer = XmlWriter.Create(context.HttpContext.Response.Output))
            {
                formatter.WriteTo(writer);

            }

        }
    }
}