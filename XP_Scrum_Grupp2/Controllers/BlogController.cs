using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace XP_Scrum_Grupp2.Controllers
{
    public class BlogController : Controller
    {
        // GET: Blog
        public ActionResult ShowBlogs()
        {
            return View();
        }
    }
}