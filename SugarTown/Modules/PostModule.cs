using System;
using Nancy.ModelBinding;
using Nancy.RouteHelpers;
using Raven.Client;

namespace SugarTown.Modules
{
    using Nancy;
    using Models;

    public class PostModule : NancyModule
    {

        private readonly IDocumentSession DocumentSession;

        public PostModule(IDocumentSession documentSession)
            : base("/posts")
        {
            DocumentSession = documentSession;

            Get["/"] = parameters =>
                           {
                               var data = DocumentSession.Query<Post>();
                               return View["Index", data];
                           };

            Get[Route.Root().AnyIntAtLeastOnce("id")] = parameters =>
                                                            {
                                                                var model = DocumentSession.Load<Post>((int)parameters.id);
                                                                return View["Edit", model];
                                                            };

            Post[Route.Root().AnyIntAtLeastOnce("id")] = parameters =>
                         {
                             var model = DocumentSession.Load<Post>((int)parameters.id);
                             this.BindTo(model);
                             DocumentSession.SaveChanges();
                             return Response.AsRedirect("/posts");
                         };

            Get["/create"] = parameters =>
                                 {
                                     var model = new Post();
                                     return View["Create", model];
                                 };

            Post["/create"] = parameters =>
            {
                var model = this.Bind<Post>();
                DocumentSession.Store(model);
                DocumentSession.SaveChanges();
                return Response.AsRedirect("/posts");
            };
        }
    }

}
