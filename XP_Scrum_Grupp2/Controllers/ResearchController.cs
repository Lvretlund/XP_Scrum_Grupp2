using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.IO;
using XP_Scrum_Grupp2.Models;

namespace XP_Scrum_Grupp2.Controllers
{
    public class ResearchController : BaseController
    {
       
        // GET: Education
        public ActionResult Index(string id)
        {

            var posts = db.ResearchBlogs.Include(x => x.Author).ToList();
            var postIndex = new ResearchIndexViewModel
            {
                EducationBlogs = posts,

            };
            return View(postIndex);
        }

        [HttpPost]
        public ActionResult Create(ResearchIndexViewModel model, HttpPostedFileBase upload)
        {
            ResearchBlog newPost = new ResearchBlog();
            var userName = User.Identity.Name;

            var author = db.Users.SingleOrDefault(x => x.UserName == userName);


            if (upload != null && upload.ContentLength > 0)
            {
                model.NewResearchBlog.Filename = upload.FileName;
                model.NewResearchBlog.ContentType = upload.ContentType;

                using (var reader = new BinaryReader(upload.InputStream))
                {
                    model.NewResearchBlog.File = reader.ReadBytes(upload.ContentLength);
                }
            }

            newPost.Author = author;
            newPost.Text = model.NewResearchBlog.Text;
            newPost.Date = DateTime.Now;
            newPost.ContentType = model.NewResearchBlog.ContentType;
            newPost.Filename = model.NewResearchBlog.Filename;
            newPost.File = model.NewResearchBlog.File;


            db.ResearchBlogs.Add(newPost);
            db.SaveChanges();
            return RedirectToAction("Index", "Education");
        }


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

    public class ResearchIndexViewModel
    {
        public string Id { get; set; }
        public ICollection<ResearchBlog> EducationBlogs { get; set; }
        public ResearchBlog NewResearchBlog { get; set; } = new ResearchBlog();

    }
}