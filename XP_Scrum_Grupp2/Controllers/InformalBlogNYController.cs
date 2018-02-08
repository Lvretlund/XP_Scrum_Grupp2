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
    public class InformalBlogNYController : BaseController
    {
        [HttpPost]
        public ActionResult HidePost(int postId, bool status)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            InformalBlog fb = db.InformalBlogs.Where(f => f.Id == postId).FirstOrDefault();
            if (status == true)
            {
                fb.Visible = false;
            }
            else
            {
                fb.Visible = true;
            }
            db.SaveChanges();
            return RedirectToAction("ShowInformalBlogs", "InformalBlogNY");
        }

        [Authorize]
        public ActionResult ShowInformalBlogs()
        {
            var posts = db.InformalBlogs.Include(x => x.Author).ToList();
            var comments = db.InformalComments.Include(f => f.InformalBlog).ToList();

            var postIndex = new PostIndexViewModel
            {
                InformalBlogs = posts,
                InformalComments = comments
            };
            return View(postIndex);
        }
 
        [Authorize]
        [HttpPost]
        public ActionResult CreateInformalPartial(PostIndexViewModel model, HttpPostedFileBase upload)
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
            newPost.Location = model.NewInformalBlog.Location;
            newPost.Visible = true;
            db.InformalBlogs.Add(newPost);
            db.SaveChanges();

            return RedirectToAction("ShowInformalBlogs", "InformalBlogNY");
        }

        [Authorize]
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