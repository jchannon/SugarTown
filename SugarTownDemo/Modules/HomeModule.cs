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
                               return View["Index"];
                           };

            Get["/blog"] = parameters =>
                               {
                                   List<Post> model = _blogRepository.GetBlogPosts(this.Context.Request.Url.ToString().Substring(0, this.Context.Request.Url.ToString().LastIndexOf("/")));

                                   return Negotiate
                                       .WithModel(model)
                                       .WithView("Blog");
                               };
        }
    }

}