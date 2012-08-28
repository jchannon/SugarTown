using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AgnosticPaging;
using SugarTown.Models;
using SugarTownDemoMVC.Model;

namespace SugarTownDemoMVC.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogRepository _blogRepository;

        private string Domain
        {
            get
            {
                return this.Request.Url.Scheme + "://" + this.Request.Url.Host +
                       ((this.Request.Url.Port == 0)
                            ? string.Empty
                            : string.Concat(":", this.Request.Url.Port));
            }
        }

        public BlogController(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public ActionResult BlogPaging(int pageNumber)
        {
            Paged<Post> model = _blogRepository.GetBlogUrlFriendlyPosts(Domain, pageNumber);

            return View("Blog", model);
        }

        public ActionResult BlogTitle(string postTitle)
        {
            Post model = _blogRepository.GetPost(postTitle, Domain);

            return View("BlogDetail", model);
        }

        public ActionResult Tag(string tagName)
        {
            return ReturnTagModelAndView(1, tagName);
        }

        public ActionResult TagPaging(string tagName, int pageNumber)
        {
            return ReturnTagModelAndView(pageNumber, tagName);
        }

        private ActionResult ReturnTagModelAndView(int pageNumber, string tagName)
        {
            Paged<Post> model = _blogRepository.GetPostsWithTags(Domain, tagName, pageNumber);
            return View("Blog", model);
        }

    }
}
