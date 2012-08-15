using System.Collections.Generic;
using Nancy;
using RestSharp;
using SugarTown.Models;
using SugarTownDemo.Model;

namespace SugarTownDemo.Modules
{
    public class HomeModule : NancyModule
    {
        private readonly IBlogRepository _blogRepository;

        public HomeModule(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;

            Get["/"] = parameters =>
                           {
                               List<Post> model = _blogRepository.GetBlogPosts(this.Context.Request.Url.ToString());

                               return Negotiate
                                   .WithModel(model)
                                   .WithView("Index");
                           };
        }
    }

}