using System.Linq;
using Nancy.Responses.Negotiation;
using SugarTown.Models;
using Nancy.ModelBinding;
using Nancy.RouteHelpers;
using Raven.Client;
using Nancy;
using System.Collections.Generic;
using SugarTown.Models.Raven;

namespace SugarTown.Modules
{

    public class PostModule : SugarTownModule
    {
        private readonly IDocumentSession DocumentSession;

        public PostModule()
            : base("/posts")
        {
            DocumentSession = new DocumentSessionProvider().GetSession();


            Get["/"] = parameters =>
                           {
                               IEnumerable<Post> data = DocumentSession.Query<Post>().ToList();
                               
                               return Negotiate
                                   .WithModel(data)
                                   .WithView("Index");

                                //return View["Index", data];
                           };

            Get[Route.Root().AnyIntAtLeastOnce("id")] = parameters =>
                            {
                                Post model = DocumentSession.Load<Post>((int)parameters.id);
                                return Negotiate
                                   .WithModel(model)
                                   .WithView("Edit");
                            };

            Post[Route.Root().AnyIntAtLeastOnce("id")] = parameters =>
                            {
                                var model = DocumentSession.Load<Post>((int)parameters.id);
                                this.BindTo(model);
                                DocumentSession.SaveChanges();
                                return Response.AsRedirect("/sugartown/posts");
                            };

            Get["/create"] = parameters =>
                            {
                                var model = new Post();
                                return Negotiate
                                   .WithModel(model)
                                   .WithView("Create");
                            };

            Post["/create"] = parameters =>
                            {
                                var model = this.Bind<Post>();
                                DocumentSession.Store(model);
                                DocumentSession.SaveChanges();
                                return Response.AsRedirect("/sugartown/posts");
                            };

        }


    }

}
