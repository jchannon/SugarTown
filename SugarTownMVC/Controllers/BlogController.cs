using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SugarTownDemoMVC.Controllers
{
    public class BlogController : Controller
    {
        public BlogController()
        {
            
        }

        public ActionResult BlogPaging(int pageNumber)
        {
            return View("Blog");
        }

        public ActionResult BlogTitle(string postTitle)
        {
            return View("BlogDetail");
        }

        public ActionResult Tag(string tagName)
        {
            return View("Blog");
        }

        public ActionResult TagPaging(string tagName, int pageNumber)
        {
            return View("Blog");
        }

    }
}
