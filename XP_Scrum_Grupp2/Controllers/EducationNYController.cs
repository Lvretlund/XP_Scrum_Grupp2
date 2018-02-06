using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XP_Scrum_Grupp2.Models;
using System.Data.Entity;

namespace XP_Scrum_Grupp2.Controllers
{
    public class EducationNYController : BaseController
    {
        // GET: Education
        [Authorize]
        public ActionResult Index(string id)
        {
            var posts = db.EducationBlogs.Include(x => x.Author).ToList();
            var postIndex = new EducationIndexViewModel
            {
                EducationBlogs = posts,

            };
            return View(postIndex);
        }
        [Authorize]
        [HttpPost]
        public ActionResult Create(EducationIndexViewModel model, HttpPostedFileBase upload)
        {
            EducationBlog newPost = new EducationBlog();
            var userName = User.Identity.Name;

            var author = db.Users.SingleOrDefault(x => x.UserName == userName);

            if (upload != null && upload.ContentLength > 0)
            {
                model.NewEducationBlog.Filename = upload.FileName;
                model.NewEducationBlog.ContentType = upload.ContentType;

                using (var reader = new BinaryReader(upload.InputStream))
                {
                    model.NewEducationBlog.File = reader.ReadBytes(upload.ContentLength);
                }
            }

            newPost.Author = author;
            newPost.Text = model.NewEducationBlog.Text;
            newPost.Date = DateTime.Now;
            newPost.ContentType = model.NewEducationBlog.ContentType;
            newPost.Filename = model.NewEducationBlog.Filename;
            newPost.File = model.NewEducationBlog.File;


            db.EducationBlogs.Add(newPost);
            db.SaveChanges();
            return RedirectToAction("Index", "EducationNY");
        }
        [Authorize]
        [HttpGet]
        public ActionResult Download(int id)
        {
            var fileItem = db.EducationBlogs.Find(id);
            if (fileItem?.File == null)
            {
                return HttpNotFound();
            }
            var response = new FileContentResult(fileItem.File, fileItem.ContentType)
            {
                FileDownloadName = fileItem.Filename
            };
            return response;
        }
    }

    public class EducationIndexViewModel
    {
        public string Id { get; set; }
        public ICollection<EducationBlog> EducationBlogs { get; set; }
        public EducationBlog NewEducationBlog { get; set; } = new EducationBlog();
    }
}