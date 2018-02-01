using XP_Scrum_Grupp2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.IO;

namespace XP_Scrum_Grupp2.Controllers
{
    public class InformalBlogController : BaseController
    {
        public ActionResult ShowInformalBlogs()
        {
            var posts = db.InformalBlogs.Include(x => x.Author).ToList();
            var postIndex = new PictureIndexViewModel
            {
                InformalBlogs = posts
            };
            return View(postIndex);
        }

        [HttpPost]
        public ActionResult CreateInformalPartial(PictureIndexViewModel model, HttpPostedFileBase upload)
        {
            InformalBlog newPost = new InformalBlog();
            var userName = User.Identity.Name;

            var author = db.Users.SingleOrDefault(x => x.UserName == userName);

            if (upload != null && upload.ContentLength > 0)
            {
                model.NewInformalBlog.Filename = upload.FileName;
                model.NewInformalBlog.ContentType = upload.ContentType;

                using (var reader = new BinaryReader(upload.InputStream))
                {
                    model.NewInformalBlog.File = reader.ReadBytes(upload.ContentLength);
                }
            }

            newPost.Author = author;
            newPost.Text = model.NewInformalBlog.Text;
            newPost.Date = DateTime.Now;
            newPost.ContentType = model.NewInformalBlog.ContentType;
            newPost.Filename = model.NewInformalBlog.Filename;
            newPost.File = model.NewInformalBlog.File;


            db.InformalBlogs.Add(newPost);
            db.SaveChanges();

            return RedirectToAction("ShowInformalBlogs", "InformalBlog");
        }

        public ActionResult Image(int id)
        {
            var post = db.InformalBlogs.Single(x => x.Id == id);
            if (post?.File == null)
            {
                return HttpNotFound();
            }
            return File(post.File, post.ContentType);
        }
    }

    public class PictureIndexViewModel
    {
        public string Id { get; set; }
        public ICollection<InformalBlog> InformalBlogs { get; set; }
        public InformalBlog NewInformalBlog { get; set; } = new InformalBlog();
    }
}
