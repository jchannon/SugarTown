using System;
using System.Collections;
using System.Linq;
using AgnosticPaging;
using Nancy.Responses.Negotiation;
using SugarTown.Models;
using Nancy.ModelBinding;
using Nancy.RouteHelpers;
using Raven.Client;
using Nancy;
using System.Collections.Generic;
using SugarTown.Models.Raven;
using Raven.Client.Linq;

namespace SugarTown.Modules
{

    public class PostModule : SugarTownModule
    {
        private readonly IDocumentSession DocumentSession;

        public PostModule()
            : base("/posts")
        {
            DocumentSession = new DocumentSessionProvider().GetSession();


            Get["/page/{pagenumber}"] = parameters =>
                           {
                               int pageNumber = parameters.pagenumber.HasValue ? parameters.pagenumber : 1;


                               var data = DocumentSession.Query<Post>()
                                   .Customize(x => x.WaitForNonStaleResultsAsOfNow())
                                   .OrderBy(x => x.DateCreated)
                                   .ThenBy(x => x.Title);


                               RavenQueryStatistics stats;
                               var pageddata = DocumentSession.Query<Post>()
                                   .Where(x => x.Published)
                                   .Customize(x => x.WaitForNonStaleResultsAsOfNow())
                                   .Statistics(out stats)
                                   .Skip((pageNumber - 1) * 25)
                                   .Take(25)
                                   .OrderBy(x => x.DateCreated)
                                   .ThenBy(x => x.Title)
                                   .ToList(); //ToList to enumerate and get stats

                               Paged<Post> jsonData =
                                   pageddata.Select(
                                       post =>
                                       new Post
                                           {
                                               Id = post.Id,
                                               Body = post.Body.Length > 1200 ? post.Body.Substring(0, 1200) + "..." : post.Body,
                                               DateCreated = post.DateCreated,
                                               Title = post.Title,
                                               Published = post.Published,
                                               Tags = post.Tags
                                           }).ToPaged(pageNumber, stats.TotalResults);

                               return Negotiate
                                   .WithModel(data)
                                   .WithMediaRangeModel("application/json", jsonData)
                                   .WithView("Index");
                           };

            Get[Route.Root().AnyIntAtLeastOnce("id")] = parameters =>
                            {
                                Post model = DocumentSession.Load<Post>((int)parameters.id);
                                return View["Edit", model];
                            };

            Post[Route.Root().AnyIntAtLeastOnce("id")] = parameters =>
                            {
                                var model = DocumentSession.Load<Post>((int)parameters.id);
                                this.BindTo(model);
                                DocumentSession.SaveChanges();
                                return Response.AsRedirect("/sugartown/posts");
                            };


            Get[Route.Root().AnyStringAtLeastOnce("title")] = parameters =>
            {
                string title = ((string)parameters.title).Replace("-", " ");
                Post model = DocumentSession.Query<Post>().FirstOrDefault(x => x.Title == title && x.Published);

                return Negotiate.WithModel(model);
            };

            Get["/tag/{tag}/page/{pagenumber}"] = parameters =>
                                                      {
                                                          string tag = (string)parameters.tag;
                                                          int pageNumber = parameters.pagenumber.HasValue ? parameters.pagenumber : 1;

                                                          RavenQueryStatistics stats;
                                                          IEnumerable<Post> model = DocumentSession.Query<Post>()
                                                              .Where(x => x.Tags.Any(tags => tags == tag) && x.Published)
                                                              .Statistics(out stats)
                                                              .Skip((pageNumber - 1) * 25)
                                                              .Take(25)
                                                              .OrderBy(x => x.DateCreated)
                                                              .ThenBy(x => x.Title)
                                                              .ToList();

                                                          Paged<Post> pagedModel = model.Select(
                                                               post =>
                                                               new Post
                                                               {
                                                                   Id = post.Id,
                                                                   Body = post.Body.Length > 1200 ? post.Body.Substring(0, 1200) + "..." : post.Body,
                                                                   DateCreated = post.DateCreated,
                                                                   Title = post.Title,
                                                                   Published = post.Published,
                                                                   Tags = post.Tags
                                                               }).ToPaged(pageNumber, stats.TotalResults);


                                                          return Negotiate.WithModel(pagedModel);
                                                      };

            Get["/create"] = parameters =>
                            {
                                var model = new Post();
                                return View["Create", model];
                            };

            Post["/create"] = parameters =>
                            {
                                var model = this.Bind<Post>();
                                model.DateCreated = DateTime.Now;
                                DocumentSession.Store(model);
                                DocumentSession.SaveChanges();
                                return Response.AsRedirect("/sugartown/posts");
                            };


        }
    }
}
