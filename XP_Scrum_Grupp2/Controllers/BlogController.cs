using XP_Scrum_Grupp2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace XP_Scrum_Grupp2.Controllers
{
    public class BlogController : BaseController
    {
        // GET: Blog
        public ActionResult ShowBlogs(string id)
        {

            var posts = db.FormalBlogs.Include(x => x.Author).ToList();
           // var dates = db.FormalBlogs.Include(x => x.Date).ToList();

            return View(new PostIndexViewModel { Id = id, FormalBlogs = posts});


        }

        [HttpPost]
        public ActionResult Create(PostIndexViewModel model)
        {
            FormalBlog newPost = new FormalBlog();
            var userName = User.Identity.Name;

            var author = db.Users.SingleOrDefault(x => x.UserName == userName);
           
            
            newPost.User = author;
            newPost.Text = model.Text;
            //newPost.Date = model.Date;
            newPost.Date = DateTime.Now;

            db.FormalBlogs.Add(newPost);
            db.SaveChanges();

            return RedirectToAction("ShowBlogs", "Blog" );
        }


    }

    public class PostIndexViewModel 
    {
        public string Id { get; set; }
        public ICollection<FormalBlog> FormalBlogs { get; set; }
        public string Text { get; set; }
        public byte File { get; set; }
        public DateTime Date { get; set; }
        public string Author_Id { get; set; }
    }

   



}