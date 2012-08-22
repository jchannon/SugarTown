using System.Collections.Generic;
using Nancy;
using Nancy.Responses.Negotiation;
using SugarTown.Infrastructure;
using SugarTown.Models;
using SugarTownDemo.Model;

namespace SugarTownDemo.Modules
{
    public class HomeModule : NancyModule
    {
        private readonly IBlogRepository _blogRepository;

        private string Domain
        {
            get
            {
                return this.Context.Request.Url.Scheme + "://" + this.Context.Request.Url.HostName +
                       ((!this.Context.Request.Url.Port.HasValue)
                            ? string.Empty
                            : string.Concat(":", this.Context.Request.Url.Port.Value));
            }
        }

        public HomeModule(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;

            Get["/"] = parameters => View["Index"];

            Get["/blog/page/{pagenumber}"] = parameters =>
                                                            {
                                                                int pageNumber = parameters.pagenumber ?? 1;

                                                                Paged<Post> model = _blogRepository.GetBlogUrlFriendlyPosts(Domain, pageNumber);

                                                                return View["Blog", model]; 
                                                            };

            Get["/blog/{title}"] = parameters =>
                                                            {
                                                                string title = (string)parameters.title;

                                                                Post model = _blogRepository.GetPost(title, Domain);

                                                                return View["BlogDetail", model];
                                                            };
            //I know you can do Get[""] = ReturnTagModelAndView; but its hard to tell which route is being invoked
            Get["/blog/tag/{tagname}"] = parameters => ReturnTagModelAndView(parameters);

            Get["/blog/tag/{tagname}/page/{pagenumber}"] = parameters => ReturnTagModelAndView(parameters);
        }

        public Negotiator ReturnTagModelAndView(dynamic parameters)
        {
            int pageNumber = parameters.pagenumber.HasValue ? parameters.pagenumber : 1;
            string tag = (string)parameters.tagname;
            Paged<Post> model = _blogRepository.GetPostsWithTags(Domain, tag, pageNumber);
            return View["Blog", model];
        }

    }

}