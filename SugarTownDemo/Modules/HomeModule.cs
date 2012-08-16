using System.Collections.Generic;
using Nancy;
using Nancy.RouteHelpers;
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
                                   string domainUrl = this.Context.Request.Url.Scheme + "://" + this.Context.Request.Url.HostName +
                                                     ( (!this.Context.Request.Url.Port.HasValue) ?
                string.Empty :
                string.Concat(":", this.Context.Request.Url.Port.Value));

                                   List<Post> model = _blogRepository.GetBlogUrlFriendlyPosts(domainUrl);

                                   return View["Blog", model];
                               };

            Get[Route.Root().Exact("blog", "blog").And().AnyStringAtLeastOnce("title")] = parameters =>
                                {
                                    string domainUrl = this.Context.Request.Url.Scheme + "://" + this.Context.Request.Url.HostName +
                                                       ((!this.Context.Request.Url.Port.HasValue) ?
                  string.Empty :
                  string.Concat(":", this.Context.Request.Url.Port.Value));
                                    string title = (string) parameters.title;
                                    
                                    
                                    Post model = _blogRepository.GetPost(title, domainUrl);

                                    return View["BlogDetail", model];
                                };
        }
    }

}