using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XP_Scrum_Grupp2.Models;

using System.Data.Entity;
using System.IO;
using System.Text;

namespace XP_Scrum_Grupp2.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index(string id)
        {
            var cat = db.Categories.ToList();
            var met = db.Meetings.Include(x => x.Creator).ToList(); //forgien key så att man kan hämta en lista av möten
            var posts = db.FormalBlogs.Include(x => x.Author).ToList();
            var postIndex = new PostIndexViewModel
            {
                FormalBlogs = posts,
                Categories = cat,
                Meetings = met
               
            };
            return View(postIndex);
        }

        
    }

   
}